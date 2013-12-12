using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using AutoMapper;
using Tuto.DataLayer.Enums;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Config;
using Tuto.Web.ViewModels;

namespace Tuto.Web.Controllers.Manager
{
    public class TutorsMgrController : DefaultController
    {
        public TutorsMgrController(WebAppLaunchContext context) : base(context)
        {
            this.setAccessType(PageAccessType.TYPE_MANAGER);
        }

        public TutorsMgrController() : this(new WebAppLaunchContext())
        {
        }

        public virtual ActionResult list()
        {
            /*if (!this.isUserAllowed())
            {
                return this.kickUser();
            }*/

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
            try
            {
                var fromAddress = new MailAddress("tutocalinours@gmail.com", "Calinours Administrateur");
                var toAddress = new MailAddress(user.mail, user.name);
                const string fromPassword = "calinours123";
                string subject = Resources.Resources.SendReasonEmailSubject;
                string body = Resources.Resources.SendReasonEmailBody + messageToSend;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception)
            {
                // TODO : manage exception
            }
        }
    }
}