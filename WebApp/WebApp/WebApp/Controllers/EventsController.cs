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

namespace WebApp.Controllers
{
    public class EventsController : BaseController
    {
        Result<int> saveResult = new Result<int>();
        IEventsService _eventService;
        ISportsService _sportsService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        public EventsController(IEventsService eventService, ISportsService sportsService, IUnitOfWorkAsync unitOfWork)
        {
            _eventService = eventService;
            _sportsService = sportsService;
            _unitOfWork = unitOfWork;
        }
        // GET: Events
        public ActionResult Index()
        {
            IEnumerable<EventsViewModels> model = new List<EventsViewModels>();
            model = _eventService.QueryableCustom().Select(s=> new EventsViewModels {
                DateCreated=s.DateCreated,
                EndDate=s.EndDate,
                EventId=s.EventId,
                EventName=s.EventName,
                SportId=s.SportId,
                SportName=s.Sport.SportName,
                StartDate=s.StartDate,
                UserId=s.UserId,
                VenueId=s.VenueId,
                VenueName=s.Venue.VenueName
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
            var resultt = _sportsService.Queryable();
            model.SportsList = resultt.data;
            return View(model);
        }

        [HttpPost]
        public ActionResult Detail(EventsViewModels model)
        {
            Events dto = Mapper.Map<Events>(model);
            dto.ObjectState = dto.EventId > 0 ? ObjectState.Modified : ObjectState.Added;
            dto.VenueId = 1;
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
                return View(model);
            }
            return RedirectToAction("Index");
        }
    }
}