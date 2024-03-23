using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
	public class ProductController : Controller
	{
		private readonly ApplicationContext _context;

		public ProductController(ApplicationContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			return View(_context.Products.ToList());
		}

		public async Task<IActionResult> Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateProduct([Bind("Id, Title, Description, Price")] Product data)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_context.Add(data);
					await _context.SaveChangesAsync();
					TempData["Message"] = "Product created successfully";
					TempData["Status"] = "Success";
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
                    TempData["Message"] = "Something went wrong";
                    TempData["Status"] = "Error";
                    return View();
                }
                return RedirectToAction("Index");
			}
			return View();
		}


		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Index");
			}
			Product product = await _context.Products.FindAsync(id) ?? throw new Exception("Id not found");
			return View(product);
		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (id is null) throw new Exception("Id not found");
			return View(await _context.Products.FindAsync(id));
		}

		[HttpPost]
		public async Task<IActionResult> EditProduct([Bind("Id, Title, Description, Price")] Product data)
		{
			if (data.Id == null) throw new Exception("The Id was not found or is incorrect");
			if (ModelState.IsValid)
			{
				_context.Update(data);
				await _context.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View();
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id is null) throw new Exception("Id not found");
			return View(await _context.Products.FindAsync(id));
		}

		[HttpPost]
		public async Task<IActionResult> DeleteProduct([Bind("Id, Title, Description, Price")] Product data)
		{
			if (data.Id == null) throw new Exception("The Id was not found or is incorrect");
			_context.Remove(data);
			try
			{
				await _context.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return View();
			}
		}
	}
}
