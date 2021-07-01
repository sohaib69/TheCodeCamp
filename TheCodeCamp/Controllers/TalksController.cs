using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using TheCodeCamp.Data;
using TheCodeCamp.Models;

namespace TheCodeCamp.Controllers
{
    [RoutePrefix("api/camps/{moniker}/talks")]
    public class TalksController : ApiController
    {
        private readonly ICampRepository _repossitory;
        private readonly IMapper _mapper;

        public TalksController(ICampRepository repo, IMapper mapper)
        {
            _repossitory = repo;
            _mapper = mapper;
        }

        [Route("")]
        public async Task<IHttpActionResult> Get(string moniker, bool includeTalks = false)
        {
            try
            {
                var result = await _repossitory.GetTalksByMonikerAsync(moniker, includeTalks);
                return Ok(_mapper.Map<IEnumerable<TalksModel>>(result));
            }
            catch (Exception ex) { return InternalServerError(ex); }
        }

        [Route("{id:int}", Name = "GetTalks")]
        public async Task<IHttpActionResult> Get(string moniker, int id, bool includeTalks = false)
        {
            try
            {
                var result = await _repossitory.GetTalkByMonikerAsync(moniker, id, includeTalks);

                if (result == null) { return NotFound(); }
                else { return Ok(_mapper.Map<IEnumerable<TalksModel>>(result)); }
            }
            catch (Exception ex) { return InternalServerError(ex); }
        }

        [Route()]
        public async Task<IHttpActionResult> Post(string moniker, TalksModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var camp = await _repossitory.GetCampAsync(moniker);

                    if (camp != null)
                    {
                        var talk = _mapper.Map<Talk>(model);
                        talk.Camp = camp;

                        // Map Speker if it's there in input.
                        if (model.Speaker != null)
                        {
                            var speaker = await _repossitory.GetSpeakerAsync(model.Speaker.SpeakerId);
                            if (speaker != null) { talk.Speaker = speaker; }
                        }

                        _repossitory.AddTalk(talk);

                        if (await _repossitory.SaveChangesAsync())
                        {
                            return CreatedAtRoute("GetTalks", new { moniker = moniker, id = talk.TalkId }, _mapper.Map<TalksModel>(talk)); ;
                        }
                    }
                }
            }
            catch (Exception ex) { return InternalServerError(ex); }

            return BadRequest(ModelState);
        }

        [Route("{talkId:int}")]
        public async Task<IHttpActionResult> Put(string moniker, int talkId, TalksModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var talk = await _repossitory.GetTalkByMonikerAsync(moniker, talkId, true);

                    if (talk == null) { return NotFound(); }

                    // This will ignoor the Speaker model.
                    _mapper.Map(model, talk);

                    // Modify speaker if needed.
                    if (talk.Speaker.SpeakerId != model.Speaker.SpeakerId)
                    {
                        var speaker = await _repossitory.GetSpeakerAsync(model.Speaker.SpeakerId);
                        if (speaker != null) { talk.Speaker = speaker; }
                    }

                    if (await _repossitory.SaveChangesAsync())
                    {
                        return Ok(_mapper.Map<TalksModel>(talk));
                    }
                }
            }
            catch (Exception ex) { return InternalServerError(ex); }

            return BadRequest(ModelState);
        }

        [Route("{talkId:int}")]
        public async Task<IHttpActionResult> Delete(string moniker, int talkId)
        {
            try
            {
                var talk = await _repossitory.GetTalkByMonikerAsync(moniker, talkId);
                if (talk == null) { return NotFound(); }

                _repossitory.DeleteTalk(talk);

                if (await _repossitory.SaveChangesAsync()) { return Ok(); }
                else { return InternalServerError(); }
            }
            catch (Exception ex) { return InternalServerError(ex); }
        }
    }
}