using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RattingSystem.Model;
using RattingSystem.Model.DTO;

namespace RattingSystem.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly RattingSystemContext _rattingSystemContext;
        public RateController(
            RattingSystemContext rattingSystemContext)
        {
            _rattingSystemContext = rattingSystemContext;
        }
        [HttpPost]
        [Route("AddRate")]
        public IActionResult AddRate(RateDTO rateDTO)
        {
            Rate rate = new Rate();
            rate.Session_Id = rateDTO.SessionId;
            rate.User_Id = rateDTO.UserId;
            rate.SessionRateDegree = rateDTO.SessionRateDegree;
            rate.PresenteRaterDegree = rateDTO.PresenterRateDegree;
            rate.Comment = rateDTO.Comment;

            _rattingSystemContext.Rate.Add(rate);
            _rattingSystemContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("ShowAllRates")]
        public IActionResult ShowAllRates()
        {
            SessionRatesDTO sessionRateVM = new SessionRatesDTO();
            List<Rate> sessionRates = _rattingSystemContext.Rate.Include(x => x.Rate_Session).Include(x => x.Rate_User).ToList();
            List<Session> Session = _rattingSystemContext.Session.ToList();
            var sessionRateAvarege = from t in _rattingSystemContext.Rate
                                     group t by new
                                     {
                                         t.Session_Id
                                     } into g
                                     select new
                                     {
                                         AverageSessionRates = g.Average(p => p.SessionRateDegree),
                                         AveragePresenterRates = g.Average(p => p.PresenteRaterDegree),
                                         g.Key.Session_Id
                                     };
            List<SessionDTO> sessionList = new List<SessionDTO>();
            foreach (var session in Session)
            {
                SessionDTO sessionObj = new SessionDTO();
                sessionObj.Id = session.Id;
                sessionObj.PresenterName = session.PresenterName;
                sessionObj.SessionName = session.SessionName;
                sessionObj.AverageSessionRates = sessionRateAvarege.Where(x => x.Session_Id == session.Id).Select(x => x.AverageSessionRates).FirstOrDefault();
                sessionObj.AveragePresenterRates = sessionRateAvarege.Where(x => x.Session_Id == session.Id).Select(x => x.AveragePresenterRates).FirstOrDefault();
                sessionList.Add(sessionObj);


            }
            sessionRateVM.SesstionList = sessionList;
            sessionRateVM.Rate = sessionRates.OrderBy(x => x.Session_Id).ToList();
            return Ok(sessionRateVM);
        }
    }
}
