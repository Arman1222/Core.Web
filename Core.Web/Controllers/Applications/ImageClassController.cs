using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Web.Controllers;
using Core.Web.Infrastructure;
using MyWeb.DataLayer;
using MyWeb.Models;
using MyWeb.ViewModels.Menus;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyWeb.Controllers
{
    [MyAuthorize(Roles = "Admin,ITSecurity")]
    public class ImageClassController : CoreControllerBase
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
              
        public async Task<ActionResult> Index()
        {
            return View(await _applicationDbContext.ImageClassSet.ToListAsync());
        }
        
        [AllowAnonymous]
        public async Task<JsonResult> All(int pageNumber = 1, int pageSize = 5)
        {
            return await ExecuteFaultHandledOperationAsync( async () =>
            {
                int totalItems = _applicationDbContext.ImageClassSet.Count();

                //var models = _applicationDbContext.ImageClassSet
                //    .Project().To<ImageClassViewModel>();

                var models = await _applicationDbContext.ImageClassSet
                .OrderByDescending(x => x.ImageClassId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

                IList<ImageClassViewModel> list = Mapper.Map<IList<ImageClass>, IList<ImageClassViewModel>>(models);

                return JsonSuccess(new { totalItems = totalItems, data = list.ToArray() });
            });
        }

        /*
         * Get All Data
         */
        [AllowAnonymous]
        public async Task<JsonResult> List()
        {
            return await ExecuteFaultHandledOperationAsync(async () =>
            {
                //var models = _applicationDbContext.ImageClassSet
                //    .Project().To<ImageClassViewModel>();

                var models = await _applicationDbContext.ImageClassSet.ToListAsync() ;

                IList<ImageClassViewModel> list = Mapper.Map<IList<ImageClass>, IList<ImageClassViewModel>>(models);

                return JsonSuccess(list);
            });
        }

        [HttpPost]        
        public async Task<JsonResult> Add(ImageClassViewModel form)
        {
            var imageclass = Mapper.Map<ImageClass>(form);
            _applicationDbContext.ImageClassSet.Add(imageclass);
            await _applicationDbContext.SaveChangesAsync();

            var model = Mapper.Map<ImageClassViewModel>(imageclass);
            return JsonSuccess(model);
        }

        [HttpPost]       
        public async Task<JsonResult> Update(ImageClassViewModel form)
        {
            var target = _applicationDbContext.ImageClassSet.Find(form.ImageClassId);
            Mapper.Map(form, target);
            await _applicationDbContext.SaveChangesAsync();

            var updatedObj = _applicationDbContext.ImageClassSet.Project().To<ImageClassViewModel>().Single(x => x.ImageClassId == form.ImageClassId);

            return JsonSuccess(updatedObj);
        }

        public ActionResult Create()
        {         
            return View();    
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ImageClassId,ImageClassName")] ImageClass imageClass)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _applicationDbContext.ImageClassSet.Add(imageClass);
                    await _applicationDbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(imageClass);
                }               
            }
            return View(imageClass);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImageClass imageClass = await _applicationDbContext.ImageClassSet.FindAsync(id);
            if (imageClass == null)
            {
                return HttpNotFound();
            }

            // Wind-up a ImageClass viewmodel
            ImageClassViewModel imageClassViewModel = new ImageClassViewModel();
            imageClassViewModel.ImageClassId = imageClass.ImageClassId;
            imageClassViewModel.ImageClassName = imageClass.ImageClassName;          
   
            return View(imageClassViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ImageClassId,ImageClassName")] ImageClassViewModel imageClassViewModel)
        {
            if (ModelState.IsValid)
            {
                // Unwind back to a ImageClass
                ImageClass editedImageClass = new ImageClass();

                try
                {                    
                    editedImageClass.ImageClassId = imageClassViewModel.ImageClassId;
                    editedImageClass.ImageClassName = imageClassViewModel.ImageClassName;                    
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);              
                    return View("Edit", imageClassViewModel);
                }

                _applicationDbContext.Entry(editedImageClass).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(imageClassViewModel);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImageClass imageClass = await _applicationDbContext.ImageClassSet.FindAsync(id);
            if (imageClass == null)
            {
                return HttpNotFound();
            }
            return View(imageClass);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ImageClass imageClass = await _applicationDbContext.ImageClassSet.FindAsync(id);

            try
            {
                _applicationDbContext.ImageClassSet.Remove(imageClass);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "You attempted to delete an image class that had menu associated with it.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View("Delete", imageClass);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _applicationDbContext.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
