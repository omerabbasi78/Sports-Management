using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModels;
using WebApp.Services;
using AutoMapper;
using WebApp.Models;
using Repository.Pattern.UnitOfWork;
using Repository.Pattern.Infrastructure;
using Repository.Pattern;
using WebApp.HelperClass;
using WebApp.Identity;

namespace WebApp.Controllers
{
    public class EventsController : BaseController
    {
        Result<int> saveResult = new Result<int>();
        IEventsService _eventService;
        ISportsService _sportsService;
        IVenueService _venueService;
        IUserChallengesService _challengeService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        public EventsController(IEventsService eventService, ISportsService sportsService, IUserChallengesService challengeService, IVenueService venueService, IUnitOfWorkAsync unitOfWork)
        {
            _eventService = eventService;
            _sportsService = sportsService;
            _challengeService = challengeService;
            _venueService = venueService;
            _unitOfWork = unitOfWork;
        }
        // GET: Events
        public ActionResult Index()
        {
            IEnumerable<EventsViewModels> model = new List<EventsViewModels>();
            model = _eventService.QueryableCustom().Where(w=>w.IsActive).Select(s=> new EventsViewModels {
                DateCreated=s.DateCreated,
                EndDate=s.EndDate,
                EventId=s.EventId,
                EventName=s.EventName,
                SportId=s.SportId,
                SportName=s.Sport.SportName,
                StartDate=s.StartDate,
                UserId=s.UserId,
                VenueId=s.VenueId,
                VenueName=s.Venue.VenueName,
                IsFree = s.IsFree,
                TotalTicketAllowed = s.TotalTicketAllowed,
                TotalBoughtTickets = s.Ticket.Where(w =>w.EventId == s.EventId && w.IsActive).Count()
            });
            return View(model);
        }


        public ActionResult Detail(int id = 0)
        {
            EventsViewModels model = new EventsViewModels();
            if (id > 0)
            {
                var result = _eventService.Find(id);
                if (result.success)
                {
                    model = Mapper.Map<EventsViewModels>(result.data);
                }
            }
            var sport = _sportsService.Queryable();
            var venue = _venueService.Queryable();
            model.SportsList = sport.data;
            model.VenueList = venue.data;
            return View(model);
        }

        [HttpPost]
        public ActionResult Detail(EventsViewModels model)
        {
            Events dto = Mapper.Map<Events>(model);
            dto.ObjectState = dto.EventId > 0 ? ObjectState.Modified : ObjectState.Added;
            dto.VenueId = model.VenueId;
            dto.SportId = model.SportId;
            dto.IsActive = true;
            dto.DateCreated = dto.EventId > 0 ? dto.DateCreated : DateTime.Now;
            dto.UserId = Common.CurrentUser.Id;
            _eventService.InsertOrUpdateGraph(dto);
            saveResult = _unitOfWork.SaveChanges();
            if (saveResult.success)
            {
                TempData["successmessage"] = "Saved Successfully.";
            }
            else
            {
                AddErrors(saveResult.errors, saveResult.ErrorMessage);
                model.SportsList = _sportsService.Queryable().data;
                model.VenueList = _venueService.Queryable().data;
                return View(model);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var result = _sportsService.Delete(id);
            saveResult = _unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Challenge(int id =0)
        {
            return PartialView("_challenge");
            //eventid
            //UsersManager manager = new UsersManager();
            //ChallengeEvent model = new ChallengeEvent();
            //model = _eventService.QueryableCustom().Where(w=>w.EventId==id).Select(s => new ChallengeEvent
            //{
            //    EndDate = s.EndDate,
            //    EventId = s.EventId,
            //    EventName = s.EventName,
            //    StartDate = s.StartDate,
            //    VenueName = s.Venue.VenueName
            //}).FirstOrDefault();
            //if (model != null)
            //{
            //    model.ToSelectedChallengesList = _challengeService.QueryableCustom().Where(w => w.EventId == id).Select(s => new ToChallenge
            //    {
            //        Name = s.ToChallenge.Name,
            //        Id = s.ToChallengeId

            //    }).ToList();
            //    model.ToChallengeList = manager.GetAllUsers();
            //}
            
            //return RedirectToAction("_challenge", model);
        }

        [HttpPost]
        public ActionResult Challenge(ChallengeEvent model)
        {
            //eventid
            
            return RedirectToAction("Index");
        }
    }
}