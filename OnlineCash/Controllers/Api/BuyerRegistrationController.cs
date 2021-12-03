using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineCash.Services;
using OnlineCash.Models.BuyerRegister;

namespace OnlineCash.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyerRegistrationController : ControllerBase
    {
        shopContext _db;
        BuyerRegistration _buyerRegistration;
        public BuyerRegistrationController(shopContext db, BuyerRegistration buyerRegistration)
        {
            _db = db;
            _buyerRegistration = buyerRegistration;
        }

        [HttpPost]
        public async Task<IActionResult> Post(VerificationCardAndPhone model)
        {
            if (!_buyerRegistration.Verification(model))
                return BadRequest();
            model = _buyerRegistration.SendSMSVerification(model);
            return Ok(model);
        }

        [HttpPut]
        public async Task<IActionResult> Put(BuyerRegistrationModel model)
        {
            if (!await _buyerRegistration.AddBuyer(model))
                return BadRequest();
            return Ok();
        }
    }
}
