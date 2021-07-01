using System;
using AutoMapper;
using System.Web.Http;
using Microsoft.Web.Http;
using TheCodeCamp.Data;
using TheCodeCamp.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TheCodeCamp.Controllers
{
    [ApiVersion("2.0")]
    [RoutePrefix("api/camps")]
    //[RoutePrefix("api/v{version:apiVersion}/camps")]
    public class Camps2Controller : ApiController
    {
        private readonly ICampRepository _repossitory;
        private readonly IMapper _mapper;

        // Sortcut to create Constructor: ctor
        public Camps2Controller(ICampRepository repo, IMapper mapper)
        {
            _repossitory = repo;
            _mapper = mapper;
        }

        [Route("")]
        public async Task<IHttpActionResult> Get(bool includeTalks = false)
        {
            try
            {
                var result = await _repossitory.GetAllCampsAsync(includeTalks);
                var mappedResults = _mapper.Map<IEnumerable<CampModel>>(result);
                return Ok(mappedResults);
            }
            catch (Exception) { return InternalServerError(); }
        }

        [Route("{moniker}", Name = "GetCamp - 2.0")]
        public async Task<IHttpActionResult> Get(string moniker)
        {
            try
            {
                var result = await _repossitory.GetCampAsync(moniker);
                if (result == null) { return NotFound(); }

                // Change the output structure. in this version.
                return Ok(new { sucess = true, camp = _mapper.Map<CampModel>(result) });
            }
            catch (Exception) { return InternalServerError(); }
        }

        [HttpGet]
        [Route("searchByDate/{eventDate:DateTime}")]
        public async Task<IHttpActionResult> SearchEventByDate(DateTime eventDate, bool includeTalks = false)
        {
            try
            {
                var result = await _repossitory.GetAllCampsByEventDate(eventDate, includeTalks);
                if (result != null) { return Ok(_mapper.Map<CampModel[]>(result)); }
                else { return NotFound(); }
            }
            catch (Exception) { return InternalServerError(); }
        }

        [Route("")]
        public async Task<IHttpActionResult> Post(CampModel model)
        {
            try
            {
                if (await _repossitory.GetCampAsync(model.Moniker) != null) { ModelState.AddModelError("Moniker", "Moniker in use."); }

                if (ModelState.IsValid)
                {
                    var camp = _mapper.Map<Camp>(model);
                    _repossitory.AddCamp(camp);
                    if (await _repossitory.SaveChangesAsync())
                    {
                        var newModel = _mapper.Map<CampModel>(camp);
                        return CreatedAtRoute("GetCamp", new { moniker = newModel.Moniker }, newModel);
                    }
                }
            }
            catch (Exception) { return InternalServerError(); }

            return BadRequest(ModelState);
        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Put(string moniker, CampModel model)
        {
            try
            {
                var camp = await _repossitory.GetCampAsync(moniker);
                if (camp == null) { return NotFound(); }

                _mapper.Map(model, camp);

                if (await _repossitory.SaveChangesAsync())
                {
                    return Ok(_mapper.Map<CampModel>(camp));
                }
                else { return InternalServerError(); }
            }
            catch (Exception) { return InternalServerError(); }
        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Delete(string moniker)
        {
            try
            {
                var camp = await _repossitory.GetCampAsync(moniker);
                if (camp == null) { return NotFound(); }

                _repossitory.DeleteCamp(camp);

                if (await _repossitory.SaveChangesAsync()) { return Ok(); }
                else { return InternalServerError(); }
            }
            catch (Exception) { return InternalServerError(); }
        }
    }
}