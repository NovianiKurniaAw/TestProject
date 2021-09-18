using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_Project.Models;

namespace Test_Project.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ReservasiList(int? idruangan = 0)
        {
            var db = new MDKAReservasiEntities1();
            var data = new List<tblT_Reservasi>();
            if (idruangan == 0)
            {
                data = db.tblT_Reservasi.ToList();
            }
            else 
            {
                data = db.tblT_Reservasi.Where(x=>x.Ruangan_FK == idruangan).ToList();
            }
            ViewBag.listRuangan = db.tblM_Ruangan.Where(x => x.Status_FK == 2).Select(x => new SelectListItem { Text = x.NamaRuangan, Value = x.Ruangan_PK.ToString() }).ToList();
            return View(data);
        }
        public ActionResult Create()
        {
            var data = new tblT_Reservasi();
            var db = new MDKAReservasiEntities1();

            ViewBag.listRuangan = db.tblM_Ruangan.Where(x=>x.Status_FK == 2).Select(x=> new SelectListItem { Text = x.NamaRuangan, Value = x.Ruangan_PK.ToString() }).ToList();
            return View(data);
        }
        public ActionResult PostCreate(tblT_Reservasi model)
        {
            var db = new MDKAReservasiEntities1();
            if (ModelState.IsValid)
            {

                var isExist = db.tblT_Reservasi.Any(x => x.TanggalReservasi == model.TanggalReservasi && x.Ruangan_FK == model.Ruangan_FK);
                if (isExist) {
                    ModelState.AddModelError("Ruangan_FK", "Tanggal dan ruangan tersebut sudah di booking");
                    return View("Create");
                }
                model.CreatedBy = "Test";
                model.CreatedDate = DateTime.Now;
                model.UpdatedBy = "Test";
                model.UpdatedDate = model.CreatedDate;
                db.tblT_Reservasi.Add(model);

                var room = db.tblM_Ruangan.FirstOrDefault(x => x.Ruangan_PK == model.Ruangan_FK);
                if (room != null)
                {
                    room.Status_FK = 1;
                }
                db.SaveChanges();
            }
            return RedirectToAction("ReservasiList", "Home");
        }
        public ActionResult Edit(int id)
        {
            var db = new MDKAReservasiEntities1();
            var data = db.tblT_Reservasi.FirstOrDefault(x=>x.Reservasi_PK == id);
            ViewBag.listRuangan = db.tblM_Ruangan.Where(x => x.Status_FK == 2).Select(x => new SelectListItem { Text = x.NamaRuangan, Value = x.Ruangan_PK.ToString() }).ToList();
            return View(data);
        }

        public ActionResult PostEdit(tblT_Reservasi model)
        {
            var db = new MDKAReservasiEntities1();
            if (ModelState.IsValid)
            {
                var existingData = db.tblT_Reservasi.FirstOrDefault(x => x.Reservasi_PK == model.Reservasi_PK);
                if (existingData != null) {
                    existingData.UpdatedBy = "Test";
                    existingData.UpdatedDate = DateTime.Now;
                    existingData.JamMulai = model.JamMulai;
                    existingData.JamSelesai = model.JamSelesai;
                    existingData.Ruangan_FK = model.Ruangan_FK;
                    existingData.SubjectReservasi = model.SubjectReservasi;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("ReservasiList", "Home");
        }
        public ActionResult Delete(int id)
        {
            var db = new MDKAReservasiEntities1();
            var data = db.tblT_Reservasi.FirstOrDefault(x=>x.Reservasi_PK.Equals(id));
            if (data != null)
            {
                var room = db.tblM_Ruangan.FirstOrDefault(x => x.Ruangan_PK == data.Ruangan_FK);
                if (room != null)
                {
                    room.Status_FK = 2;
                }
                db.tblT_Reservasi.Remove(data);
                db.SaveChanges();
            }
            return RedirectToAction("ReservasiList", "Home");
        }
    }
}