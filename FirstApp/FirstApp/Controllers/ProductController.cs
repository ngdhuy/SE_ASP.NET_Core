using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FirstApp.Libs;
using FirstApp.Models;
using Microsoft.AspNetCore.Session;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FirstApp.Controllers
{
    public class ProductController : Controller
    {
        private List<Product> Products = new List<Product>();

        public void GenerateData()
        {

            if (HttpContext.Session.Keys.Cast<String>().Where(p => p.Contains("Products")).SingleOrDefault() == null)
            {
                for (int i = 0; i < 10; i++)
                {
                    Product p = new Product();
                    p.ID = i;
                    p.Name = "Product name " + i.ToString();
                    p.Price = i * 0.5;

                    Products.Add(p);
                }

                HttpContext.Session.Set<List<Product>>("Products", Products);
            }
            else
            {
                Products = HttpContext.Session.Get<List<Product>>("Products");
            }
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            GenerateData();
            return View(Products);
        }

        public IActionResult Detail(int id)
        {
            GenerateData();
            Product p = Products.Single(p => p.ID == id);
            return View(p);
        }

        public IActionResult Delete(int id)
        {
            Products = HttpContext.Session.Get<List<Product>>("Products");

            Product product = Products.Single(p => p.ID == id);
            Products.Remove(product);
            HttpContext.Session.Set<List<Product>>("Products", Products);
            return RedirectToAction("index");
        }

        // -- GET: Product/Create -> Call Form CREATE
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                Products = HttpContext.Session.Get<List<Product>>("Products");

                Products.Add(product);
                HttpContext.Session.Set<List<Product>>("Products", Products);

                return RedirectToAction("index");
            }
            else
                return View();
            
        }

        // -- GET: Product/Eit/id -> Call From EDIT
        public IActionResult Edit(int id)
        {
            Products = HttpContext.Session.Get<List<Product>>("Products");

            Product product = Products.Single(p => p.ID == id);

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                Products = HttpContext.Session.Get<List<Product>>("Products");

                Product prod = Products.Single(p => p.ID == product.ID);
                prod.Name = product.Name;
                prod.Price = product.Price;
                
                HttpContext.Session.Set<List<Product>>("Products", Products);

                return RedirectToAction("index");
            }
            else
                return View();
        }

        public IActionResult Search(String name, int size)
        {
            ViewBag.productName = name;
            ViewBag.productSize = size;
            return View();
        }
    }
}
