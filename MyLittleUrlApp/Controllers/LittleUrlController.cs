using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using MyLittleUrlApp.Models;
using MyLittleUrlApp.ApiHelpers;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyLittleUrlApp.Controllers
{
    public class LittleUrlController : Controller
    {
        private static MyLittleUrlWebAPIHelper _apiHelper;

        private static MyLittleUrlWebAPIHelper GetUrlHelper()
        {
            return new MyLittleUrlWebAPIHelper();
        }

        public LittleUrlController()
        {
            if(_apiHelper == null)
                _apiHelper = GetUrlHelper();
        }

        // GET: /<controller>/
        public IActionResult Load(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                string sResult = _apiHelper.GetUrlByKey(key).Result;
                if (!string.IsNullOrEmpty(sResult))
                {
                    LittleUrl url = JsonConvert.DeserializeObject<LittleUrl>(sResult);
                    return Redirect(url.LongUrl);
                }
                else
                {
                    TempData.Add("ErrorMessage", "Your Little Url was not found in store.");
                    return RedirectToAction("Error");
                }
            }
                
            return RedirectToAction("Index");
        }

        // GET: /<controller>/
        public IActionResult Index(string key = null)
        {
            try
            {
                if(!string.IsNullOrEmpty(key))
                {
                    string sResult = _apiHelper.GetUrlByKey(key).Result;
                    if (!String.IsNullOrEmpty(sResult))
                    {
                        TempData["urlResult"] = sResult;
                        return RedirectToAction("Show");
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                TempData.Add("ErrorMessage", String.IsNullOrEmpty(ex.InnerException?.Message ?? "") ? ex.Message : ex.InnerException.Message);
                return RedirectToAction("Error");
            }
        }

        public IActionResult Show()
        {
            // Get result from tempData
            string sResult = (string)TempData["urlResult"];
            if (!String.IsNullOrEmpty(sResult))
            {
                LittleUrl url = JsonConvert.DeserializeObject<LittleUrl>(sResult);

                // Set base address
                url.BaseAddressPrefix = MyLittleUrlWebAPIHelper.BaseAddressPrefix;

                // set ReductionRate
                float fVal = 0;
                if (!String.IsNullOrEmpty(url.LongUrl) && !String.IsNullOrEmpty(url.ShortUrl))
                    fVal = 100 * (((float)url.LongUrl.Length - (float)(url.BaseAddressPrefix.Length + url.ShortUrl.Length)) / (float)url.LongUrl.Length);
                TempData["ReductionRate"] = fVal.ToString("0.00");

                return View(url);
            }

            return RedirectToAction("Index");
        }

        // POST
        public IActionResult Create(string urlToEncode)
        {
            try
            {
                if (string.IsNullOrEmpty(urlToEncode))
                    return RedirectToAction("Index");
                
                if (urlToEncode.StartsWith(MyLittleUrlWebAPIHelper.BaseAddressPrefix, StringComparison.CurrentCultureIgnoreCase))
                {
                    TempData.Add("ErrorMessage", "Invalid attempt.  This is already a LittleURL.");
                    return RedirectToAction("Error");
                }
                   
                string sResult = _apiHelper.CreateNewUrl(urlToEncode).Result;
                LittleUrl url = JsonConvert.DeserializeObject<LittleUrl>(sResult);
                return RedirectToAction("Index", new { key = url.ShortUrl });
            }
            catch (Exception ex)
            {
                TempData.Add("ErrorMessage", String.IsNullOrEmpty(ex.InnerException?.Message ?? "") ? ex.Message : ex.InnerException.Message);
                return RedirectToAction("Error");
            }
        }

        // GET
        [Authorize]
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
                TempData.Add("ErrorMessage", String.IsNullOrEmpty(ex.InnerException?.Message ?? "") ? ex.Message : ex.InnerException.Message);
                return RedirectToAction("Error");
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
                    if (!string.IsNullOrEmpty(sResult))
                    {
                        LittleUrl url = JsonConvert.DeserializeObject<LittleUrl>(sResult);
                        return View(url);
                    }
                    else
                    {
                        ViewData["ErrorMessage"] = "Your Little Url was not found in store.";
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                TempData.Add("ErrorMessage", String.IsNullOrEmpty(ex.InnerException?.Message ?? "") ? ex.Message : ex.InnerException.Message);
                return RedirectToAction("Error");
            }
        }

        // Boilerplate
        public IActionResult Error()
        {
            string message = TempData["ErrorMessage"].ToString();
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = message });
        }
    }
}
