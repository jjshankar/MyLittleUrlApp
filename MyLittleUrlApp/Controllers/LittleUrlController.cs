﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using MyLittleUrlApp.Models;
using MyLittleUrlApp.ApiHelpers;
using Newtonsoft.Json;
using System.Diagnostics;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyLittleUrlApp.Controllers
{
    public class LittleUrlController : Controller
    {
        private MyLittleUrlWebAPIHelper _apiHelper;

        public LittleUrlController()
        {
            _apiHelper = new MyLittleUrlWebAPIHelper();
        }

        // GET: /<controller>/
        public IActionResult Index(string key = null)
        {
            try
            {
                if(!string.IsNullOrEmpty(key))
                {
                    string sResult = _apiHelper.GetUrlByKey(key).Result;
                    LittleUrl url = JsonConvert.DeserializeObject<LittleUrl>(sResult);
                    return View(url);
                }

                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", ex.Message);
            }
        }

        // POST
        public IActionResult Create(string urlToEncode)
        {
            try
            {
                if(string.IsNullOrEmpty(urlToEncode))
                    return RedirectToAction("Index");
                
                string sResult = _apiHelper.CreateNewUrl(urlToEncode).Result;
                LittleUrl url = JsonConvert.DeserializeObject<LittleUrl>(sResult);
                return RedirectToAction("Index", new { key = url.ShortUrl });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = ex.Message });
            }
        }

        // GET
        public IActionResult List()
        {
            try
            {
                string sResult = _apiHelper.GetAllUrls().Result;
                List<LittleUrl> urlList = JsonConvert.DeserializeObject<List<LittleUrl>>(sResult);
                return View(urlList);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = ex.Message });
            }
        }

        // GET:
        public IActionResult Fetch(string urlkey = null)
        {
            try
            {
                if(!string.IsNullOrEmpty(urlkey))
                {
                    string sResult = _apiHelper.GetUrlByKey(urlkey).Result;
                    LittleUrl url = JsonConvert.DeserializeObject<LittleUrl>(sResult);
                    return View(url);
                }

                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = ex.Message });
            }
        }

        // Boilerplate
        public IActionResult Error(string message)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = message });
        }
    }
}
