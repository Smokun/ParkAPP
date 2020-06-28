using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkWeb.Models;
using ParkWeb.Repository.IRepository;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ParkWeb.Controllers
{
    public class NationalParksController : Controller
    {

        private readonly INationalParkRepository _npRepo;

        public NationalParksController(INationalParkRepository npRepo)
        {
            _npRepo = npRepo;
        }

        public IActionResult Index()
        {
            return View(new NationalPark() { });
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            NationalPark obj = new NationalPark();
            if (id == null)
            {
                //will be true for inser/create
                return View(obj);
            }
            //true for update
            obj = await _npRepo.GetAsync(StaticDetails.NationalParkAPIPath, id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark obj)
        {
            //if true all required data was provided properly
            if (ModelState.IsValid)
            {
                //processing the uploaded files
                var files = HttpContext.Request.Form.Files;
                //if true image was uploaded
                if (files.Count > 0)
                {
                    //img converting to string
                    byte[] p1 = null;
                    //opening the file
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        //converting the img to array of bytes
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();

                        }
                    }
                    //picture == array of bytes
                    obj.Picture = p1;
                }
                else
                {
                    //retrive obj from db and get the image
                    var objFromDb = await _npRepo.GetAsync(StaticDetails.NationalParkAPIPath, obj.Id);
                    obj.Picture = objFromDb.Picture;


                }
                if (obj.Id == 0)
                {
                    //update expects id
                    await _npRepo.CreateAsync(StaticDetails.NationalParkAPIPath + obj.Id, obj);
                }
                return RedirectToAction(nameof(Index));
            }
            //if the model state is not valid
            else
            {
                return View(obj);
            }
            

        }
        public async Task<IActionResult> GetAllNationalPark()
        {   //we will be loading data using API
            return Json(new { data = await _npRepo.GetAllAsync(StaticDetails.NationalParkAPIPath) });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {   
            var status = await _npRepo.DeleteAsync(StaticDetails.NationalParkAPIPath, id);
            if (status)
            {
                return Json(new { success = true, message="Delete successful" });
            }
            return Json(new { success = false, message = "Delete not successful" });
        }


    }
}
