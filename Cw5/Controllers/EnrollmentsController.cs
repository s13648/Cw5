﻿using System.Net;
using System.Threading.Tasks;
using Cw5.Dal;
using Cw5.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization.Internal;

namespace Cw5.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IStudyDbService studyDbService;
        private readonly IStudentDbService studentDbService;
        private readonly IEnrollmentDbService enrollmentDbService;

        public EnrollmentsController(IStudyDbService studyDbService, IStudentDbService studentDbService, IEnrollmentDbService enrollmentDbService)
        {
            this.studyDbService = studyDbService;
            this.studentDbService = studentDbService;
            this.enrollmentDbService = enrollmentDbService;
        }

        [HttpPost]
        public async Task<IActionResult> EnrollStudent(EnrollStudent model)
        {
            var study = await studyDbService.GetByName(model.Studies);
            if (study == null)
                return BadRequest($"Study with name: {model.Studies} not found");

            if (await studentDbService.Exists(model.IndexNumber))
                return BadRequest($"Student with index: {model.Studies} already exists");

            var enrollment = await enrollmentDbService.EnrollStudent(model,study);

            return StatusCode(201, enrollment);
        }

        [HttpPost("promotions")]
        public async Task<IActionResult> Promotions(Promotions promotions)
        {
            if (!await enrollmentDbService.Exists(promotions.Studies, promotions.Semester))
                return BadRequest("Promotion not found");


            await enrollmentDbService.Promotions(promotions);

            return StatusCode(201, await enrollmentDbService.GetBy(promotions.Studies, promotions.Semester + 1));
        }
    }
}
