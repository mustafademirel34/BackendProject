﻿using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login(UserForLoginDto userForLoginDto) // giriş bilgileri
        {
                                                                    // dependecy resolver
            var userToLogin = _authService.Login(userForLoginDto); // servis ile manager üzerinden giriş yapılıyor, 
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message); // giriş hatalı ise hata döndürür
            }

            var result = _authService.CreateAccessToken(userToLogin.Data); // kullanıcının bilgisiyle bir token oluşturuluyor
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("register")]
        public IActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            var userExists = _authService.UserExists(userForRegisterDto.Email); // kayıt aşamasında önce kullanıcı daha önce var mı diye kontrol ediyor
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }

            var registerResult = _authService.Register(userForRegisterDto, userForRegisterDto.Password); // kayıt gerçekleştiriyor (bu işlemler business'da)
            var result = _authService.CreateAccessToken(registerResult.Data);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
    }
}
