using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using AutoMapper;
using Tuto.DataLayer.Enums;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Config;
using Tuto.Web.ViewModels.TutorsMgr;

namespace Tuto.Web.Controllers.Manager
{
    public class TutorsMgrController : DefaultController
    {
        public TutorsMgrController(WebAppLaunchContext context) : base(context)
        {
            this.setAccessType(PageAccessType.TYPE_MANAGER);
        }

        public TutorsMgrController() : this(new WebAppLaunchContext())
        { }

        public virtual ActionResult list()
        {
            if (!this.isUserAllowed())
            {
                return this.kickUser();
            }

            var tutors = this.appContext.getRepository().getAll<Tutor>().ToList();


            var tutorListViewModels = new List<TutorListViewModel>();
            foreach (var tutor in tutors)
            {
                var newTutor = Mapper.Map<TutorListViewModel>(tutor);

                tutorListViewModels.Add(newTutor);
            }
            return View("TutorList", tutorListViewModels);
        }

        public virtual ActionResult tutorDetails(int id)
        {
            if (!this.isUserAllowed())
            {
                return this.kickUser();
            }

            var tutor = this.appContext.getRepository().getById<Tutor>(id);
            var tutorViewModel = Mapper.Map<TutorDetailsViewModel>(tutor);

            return View("TutorDetails", tutorViewModel);
        }

        public ActionResult setStateConfirmed(int tutorId)
        {
            var selectedTutor = this.appContext.getRepository().getById<Tutor>(tutorId);
            selectedTutor.tutorState = TutorState.CONFIRMED;

            this.appContext.getRepository().update(selectedTutor);
            return this.list();
        }

        public ActionResult setStateInactive(int tutorId, string message)
        {

            var selectedTutor = this.appContext.getRepository().getById<Tutor>(tutorId);
            selectedTutor.tutorState = TutorState.INACTIVE;

            this.appContext.getRepository().update(selectedTutor);
            return this.list();
        }

        [HttpPost]
        public void sendReasonEmail(User user, string messageToSend)
        {
            var toAddress = new MailAddress(user.mail, user.name);
            var emailToSend = new MailMessage(WebAppConfiguration.SmtpEmailConfiguration.SEND_FROM_ADDRESS, toAddress)
            {
                Subject = Resources.Resources.SendReasonEmailSubject,
                Body = new StringBuilder(Resources.Resources.SendReasonEmailBody)
                            .Append(messageToSend)
                            .ToString()
            };

            this.appContext.getConfiguration().mailSender.sendMail(emailToSend);
        }
    }
}