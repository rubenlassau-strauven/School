using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.Data;

namespace MusicStore.Controllers
{
    public class StoreController : Controller
    {
        private IGenreRepository _genreRepository;
        private IAlbumRepository _albumRepository;
        private IAlbumViewModelFactory _albumViewModelFactory;

        public StoreController(IGenreRepository _genreRepository, IAlbumRepository _albumRepository, IAlbumViewModelFactory _albumViewModelFactory)
        {
            this._genreRepository = _genreRepository;
            this._albumRepository = _albumRepository;
            this._albumViewModelFactory = _albumViewModelFactory;
        }

        // GET: Store
        public ActionResult Index()
        {
            var allGenres = _genreRepository.GetAll();
            return View(allGenres);
        }

        // GET: Store/Details/5
        public ActionResult Details(int id)
        {
            var album = _albumRepository.GetById(id);
            if (album != null)
            {
                var genre = _genreRepository.GetById(album.GenreId);
                var model = _albumViewModelFactory.Create(album, genre);
                return View(model);
            }
            ;
            return new HttpNotFoundResult();
        }

        // GET: Store/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Store/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Store/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Store/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Store/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Store/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Browse(int genreId)
        {
            var genre = _genreRepository.GetById(genreId);
            if (genre != null)
            {
                var albumsOfCertainGenre = _albumRepository.GetByGenre(genreId);
                ViewBag.Genre = genre.Name;
                return View(albumsOfCertainGenre);
            }
            return new HttpNotFoundResult();
        }

        [ChildActionOnly]
        public ActionResult AlbumOfTheDay()
        {
            var album = new AlbumViewModel()
            {
                Artist = "A popular artist",
                Genre = "Some genre",
                Title = "Some title"
            };

            return PartialView("_Album", album);
        }

    }
}
