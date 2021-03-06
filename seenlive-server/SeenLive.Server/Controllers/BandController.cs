﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SeenLive.Server.DTOs;
using SeenLive.DataAccess.Models;
using SeenLive.DataAccess.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeenLive.Server.Controllers
{  
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BandController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IArtistService _artistService;
        private readonly IDatesService _datesService;

        public BandController(IMapper mapper, IArtistService artistService, IDatesService datesService)
        {
            _mapper = mapper;
            _artistService = artistService;
            _datesService = datesService;
        }

        [HttpPost]
        public ActionResult<IEnumerable<ArtistResponseDTO>> AddArtistEntry(ArtistCreationRequestDTO artistRequest)
        {
            if (artistRequest == null)
                return BadRequest("Artist creation request is missing");

            try
            {
                // TODO: extract this into a business logic / CQRS layer
                
                ArtistEntry artistEntry = _artistService.Get().SingleOrDefault(entry => entry.ArtistName == artistRequest.ArtistName);
                IEnumerable<string> dateEntryIDs = CreateDateEntries(artistRequest.DateEntryRequests);

                if (artistEntry == null)
                {
                    artistEntry = new ArtistEntry(string.Empty, artistRequest.ArtistName, dateEntryIDs);
                    _artistService.Create(artistEntry);
                }
                else
                {
                    artistEntry.AddDateEntries(dateEntryIDs);
                    _artistService.Update(artistEntry.Id, artistEntry);
                }

                return Ok(GetArtistEntriesAsDTO());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public ActionResult DeleteDateEntry(DateEntryDeleteRequestDTO deletionRequest)
        {
            if (deletionRequest == null)
                return BadRequest("Deletion request is missing");

            try
            {
                ArtistEntry artist = _artistService.Get(deletionRequest.ArtistId);
                bool removeResult = _datesService.Remove(deletionRequest.DateId);
                artist.DateEntryIDs.Remove(deletionRequest.DateId);

                if (artist.DateEntryIDs.Count > 0)
                {                    
                    _artistService.Update(deletionRequest.ArtistId, artist);

                    return removeResult ? Ok() : NotFound() as ActionResult;
                }

                return DeleteArtistEntry(artist.Id)
                    ? Ok()
                    : NotFound() as ActionResult;
            }
            catch (Exception)
            {
                return BadRequest();
            }            
        }

        [HttpPost]
        public ActionResult DeleteArtistEntry(ArtistDeleteRequestDTO request)
        {
            if (request == null)
                return BadRequest("Deletion request is missing");

            try
            {
                return DeleteArtistEntry(request.ArtistEntryId) 
                    ? Ok() 
                    : NotFound() as ActionResult;
            }
            catch(Exception)
            {
                return BadRequest();
            }            
        }

        private bool DeleteArtistEntry(string artistEntryId)
        {
            ArtistEntry artist = _artistService.Get(artistEntryId);
            foreach (string dateId in artist.DateEntryIDs)
            {
                _datesService.Remove(dateId);
            }
            return _artistService.Remove(artistEntryId);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ArtistResponseDTO>> GetArtistEntries()
        {
            try
            {
                return Ok(GetArtistEntriesAsDTO());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        private IEnumerable<string> CreateDateEntries(IEnumerable<DateEntryCreationRequestDTO> requests)
        {
            return requests.Select(request =>
            {
                DateEntry newDateEntry = _datesService.Create(_mapper.Map<DateEntry>(request));
                return newDateEntry?.Id;
            });
        }

        private IEnumerable<ArtistResponseDTO> GetArtistEntriesAsDTO()
        {
            var ret = _artistService.Get().Select(entry => _mapper.Map<ArtistResponseDTO>(entry));
            return ret;
        }
    }
}