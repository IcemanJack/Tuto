using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Models.Notifications.Manager;
using Tuto.DataLayer.Models.Users;
using Tuto.DataLayer.ModelUtilities;
using Tuto.Web.Config;
using Tuto.Web.Controllers.Utilities;
using Tuto.Web.ViewModels.Account;
using Tuto.Web.ViewModels.Account.Edit;
using Tuto.Web.ViewModels.Account.Register;

namespace Tuto.Web.Controllers
{
    public partial class AccountController : DefaultController
    {
        public AccountController(WebAppLaunchContext context) : base(context)
        {
            this.setAccessType(PageAccessType.TYPE_USER);
        }

        public AccountController() : this(new WebAppLaunchContext())
        {}

        public virtual ActionResult index()
        {
            return this.View("Login");
        }

        public virtual ActionResult unauthorized()
        {
            this.ModelState.AddModelError("error", Resources.Resources.UnauthorizedAccess);
            return this.View("Login");
        }

        public virtual ActionResult login()
        {
            this.disconnectLoggedInUser(); // make sure we disconnect the current user
            return this.View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult login(UserLoginViewModel userLoginViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("Login");
            }

            var user = AccountLoginUtilities.getUserFromLogin(this.appContext.getRepository(), userLoginViewModel.mail);

            if (user == null)
            {
                this.ModelState.AddModelError("", Resources.Resources.ErrorLogin);
                return this.View("Login");
            }

            this.setLoggedInUser(user);
            return this.RedirectToAction("index", "Home");
        }

        public virtual ActionResult disconnect()
        {
            return this.disconnectLoggedInUser();
        }


        public virtual ActionResult registerTutor()
        {
            var departments = this.appContext.getRepository().getAll<Department>().ToArray();
            var deparmentsDictionnary = new Dictionary<string, string[]>();

            foreach (var department in departments)
            {
                var coursesName = department.courses.Select(course => course.courseName).ToArray();
                deparmentsDictionnary[department.name] = coursesName;
            }

            this.ViewBag.Departments = JsonConvert.SerializeObject(deparmentsDictionnary, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return this.View("RegisterTutor");
        }

        [HttpPost]
        public virtual ActionResult registerTutor(TutorRegisterViewModel tutorRegisterViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("registerTutor");
            }

            var tutor = Mapper.Map<Tutor>(tutorRegisterViewModel);

            // schedule blocks
            var scheduleBlocks = ScheduleUtilities.getScheduleBlocksFromJson(tutorRegisterViewModel.scheduleBlocksJson, this.appContext.getRepository());
            tutor.scheduleBlocks = scheduleBlocks;

            // courses skills
            foreach (var course in tutorRegisterViewModel.getCoursesSkills(this.appContext.getRepository()))
            {
                tutor.coursesSkills.Add(this.appContext.getRepository().getById<Course>(course.id));
            }

            if (this.emailVerification(tutor))
            {
                this.sendConfirmationEmail(tutor);
            }

            this.appContext.getRepository().add(tutor);

            // notification
            var newTutorNotification = new TutorHasRegisteredTask().getBuilder().setNewlyRegistredTutor(tutor).setConcernedManager(this.appContext.getConfiguration().mainManager).getNotification();
            this.appContext.getRepository().add(newTutorNotification);

            return this.RedirectToAction("login", "Account");
        }

        public virtual ActionResult registerHelped()
        {
            return this.View("RegisterHelped");
        }

        [HttpPost]
        public virtual ActionResult registerHelped(HelpedRegisterViewModel helpedRegisterViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.registerHelped();
            }

            // schedule blocks
            var scheduleBlocks = ScheduleUtilities.getScheduleBlocksFromJson(helpedRegisterViewModel.scheduleBlocksJson, this.appContext.getRepository());

            var helped = Mapper.Map<Helped>(helpedRegisterViewModel);
            helped.scheduleBlocks = scheduleBlocks;

            if (this.emailVerification(helped))
            {
                this.sendConfirmationEmail(helped);
            }

            this.appContext.getRepository().add(helped);

            // notification
            var newHelpedNotification =
                new HelpedHasRegisteredAlert().getBuilder()
                    .setConcernedHelped(helped)
                    .setConcernedManager(this.appContext.getConfiguration().mainManager)
                    .getNotification();
            this.appContext.getRepository().add(newHelpedNotification);

            return this.RedirectToAction("login", "Account");
        }

        public virtual ActionResult edit()
        {
            if (!this.isLoggedIn())
            {
                return this.kickUser();
            }

            var loggedInUser = this.getLoggedInUser();

            if (loggedInUser.isHelped())
            {
                return this.edit((Helped)loggedInUser);
            }
            else if (loggedInUser.isTutor())
            {
                return this.edit((Tutor)loggedInUser);
            }
            else
            {
                return this.edit((Tuto.DataLayer.Models.Users.Manager)loggedInUser);
            }
        }

        private ActionResult edit(Helped helped)
        {
            var editHelpedViewModel = Mapper.Map<EditHelpedViewModel>(helped);

            return View("EditHelped", editHelpedViewModel);
        }

        private ActionResult edit(Tutor tutor)
        {
            var editTutorViewModel = Mapper.Map<EditTutorViewModel>(tutor);

            return View("EditTutor", editTutorViewModel);
        }

        private ActionResult edit(Tuto.DataLayer.Models.Users.Manager manager)
        {
            var editManagerViewModel = Mapper.Map<EditManagerViewModel>(manager);

            return View("EditManager", editManagerViewModel);
        }

        [HttpPost]
        public virtual ActionResult editHelped(EditHelpedViewModel editHelpedViewModel)
        {
            return this.generalEdit(editHelpedViewModel);
        }

        [HttpPost]
        public virtual ActionResult editTutor(EditTutorViewModel editTutorViewModel)
        {
            return this.generalEdit(editTutorViewModel);
        }

        [HttpPost]
        public virtual ActionResult editManager(EditManagerViewModel editManagerViewModel)
        {
            return this.generalEdit(editManagerViewModel);
        }

        private ActionResult generalEdit(EditUserViewModel editUserViewModel)
        {
            if (!this.isLoggedIn()) return this.kickUser();
            if (!this.ModelState.IsValid) return this.edit();

            var loggedInUser = this.getLoggedInUser();
            if (editUserViewModel.currentEmail != loggedInUser.mail || editUserViewModel.currentPassword != loggedInUser.password)
            {
                this.ViewData["CurrentInfoError"] = Resources.Resources.ErrorEditWrongCurrentInfo;
                return this.edit();
            }

            var repository = this.appContext.getRepository();
            var updatedUser = AccountEditUtilities.makeNewOnlineUserFromViewModel(this.getLoggedInUser(), editUserViewModel, repository);

            if (updatedUser.isTutor())
            {
                repository.update(updatedUser as Tutor);
            }
            else if (updatedUser.isHelped())
            {
                repository.update(updatedUser as Helped);
            }
            else
            {
                repository.update(updatedUser as Tuto.DataLayer.Models.Users.Manager);
            }

            this.ViewData["AccountEditSuccessMessage"] = Resources.Resources.ChangesHaveBeenSaved;
            this.updateSessionCreditentials(updatedUser.mail, updatedUser.password);

            return this.edit(); 
        }

        [HttpPost]
        public void sendConfirmationEmail(User user)
        {
            var toAddress = new MailAddress(user.mail, user.name);
            var emailToSend = new MailMessage(WebAppConfiguration.SmtpEmailConfiguration.SEND_FROM_ADDRESS, toAddress)
            {
                Subject = Resources.Resources.SendConfirmationEmailSubject,
                Body = Resources.Resources.SendConfirmationEmailBody
            };

            this.appContext.getConfiguration().mailSender.sendMail(emailToSend);
        }

        public bool emailVerification(User user)
        {
            var adress = user.mail;
            try
            {
                var addr = new System.Net.Mail.MailAddress(adress);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
