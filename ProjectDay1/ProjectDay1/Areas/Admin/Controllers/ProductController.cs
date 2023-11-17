using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Model;
using UDW.Library;

namespace ProjectDay1.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        ProductsDAO productsDAO = new ProductsDAO();
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        SuppliersDAO suppliersDAO = new SuppliersDAO();

        // GET: Admin/Product
        public ActionResult Index()
        {
            return View(productsDAO.getList("Index"));
        }

        // GET: Admin/Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tồn tại sản phẩm");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                TempData["message"] = new XMessage("danger", "Không tồn tại sản phẩm");
                return RedirectToAction("Index");
            }
            return View(products);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.ListCatID = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.ListSupID = new SelectList(productsDAO.getList("Index"), "Id", "Name");
            //dung de lua chon tu danh sách droplist nhu bảng CAtegories: ParentID va Supplier: ParentID
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Products products)
        {
            if (ModelState.IsValid)
            {
                //xu li thong tin tu dong
                products.CreatedAt = DateTime.Now;
                //Xu ly tu dong: UpdateAt
                products.UpdateAt = DateTime.Now;
                //Xu ly tu dong: CreateBy
                products.CreatedBy = Convert.ToInt32(Session["UserId"]);
                //Xu ly tu dong: UpdateBy
                products.UpdateBy = Convert.ToInt32(Session["UserId"]);
                //Xu ly tu dong: Slug
                products.Slug = XString.Str_Slug(products.Name);

                //xu ly cho phan upload hinh anh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = products.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        products.Image = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/supplier";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh
                productsDAO.Insert(products);
                TempData["message"] = new XMessage("danger", "Không tồn tại sản phẩm");
                return RedirectToAction("Index");
            }
            ViewBag.ListCatID = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.ListSupID = new SelectList(productsDAO.getList("Index"), "Id", "Name");
            return View(products);
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tồn tại sản phẩm");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                TempData["message"] = new XMessage("danger", "Không tồn tại sản phẩm");
                return RedirectToAction("Index");
            }
            return View(products);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Products products)
        {
            if (ModelState.IsValid)
            {
                //xu ly tu dong cho cac truong: Slug, CreateAt/By, UpdateAt/By, Oder
                //Xu ly tu dong: UpdateAt
                products.UpdateAt = DateTime.Now;
                //Xu ly tu dong: Slug
                products.Slug = XString.Str_Slug(products.Name);

                //xu ly cho phan upload hinh anh
                var img = Request.Files["img"];//lay thong tin file
                string PathDir = "~/Public/img/supplier";
                if (img.ContentLength != 0)
                {
                    //Xu ly cho muc xoa hinh anh
                    if (products.Image != null)
                    {
                        string DelPath = Path.Combine(Server.MapPath(PathDir), products.Image);
                        System.IO.File.Delete(DelPath);
                    }

                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = products.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        products.Image = imgName;
                        //upload hinh
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }

                }//ket thuc phan upload hinh anh

                //cap nhat mau tin vao DB
                productsDAO.Update(products);
                //thong bao tao mau tin thanh cong
                TempData["message"] = new XMessage("success", "Cập nhật sản phẩm thành công");
                return RedirectToAction("Index");
            }
            return View(products);
        }

        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tồn tại sản phẩm");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                TempData["message"] = new XMessage("danger", "Không tồn tại sản phẩm");
                return RedirectToAction("Index");
            }
            return View(products);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = productsDAO.getRow(id);
            productsDAO.Delete(products);
            TempData["message"] = new XMessage("success", "Cập nhật sản phẩm thành công");
            return RedirectToAction("Trash");
        }

        //STATUS
        //GET: Admin/Category/Status/5
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //thoong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            //Truy vấn id
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            else
            {
                //chuyển đổi trạng thái status 1<->2
                products.Status = (products.Status == 1) ? 2 : 1;

                //Cập nhật UpdateAt
                products.UpdateAt = DateTime.Now;

                //Cập nhật lại data
                productsDAO.Update(products);

                //Thông báo cập nhật trạng thái thành công
                TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
                return RedirectToAction("Index");
            }
        }

        //Thùng rác
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //thoong bao that bai
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
            //Truy vấn id
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
            else
            {
                //chuyển đổi trạng thái status 1,2 -> 0
                products.Status = 0;

                //Cập nhật UpdateAt
                products.UpdateAt = DateTime.Now;

                //Cập nhật lại data
                productsDAO.Update(products);

                //Thông báo cập nhật trạng thái thành công
                TempData["message"] = new XMessage("success", "Xoá mẫu tin thành công");
                return RedirectToAction("Index");
            }
        }

        //Trash
        public ActionResult Trash()
        {
            return View(productsDAO.getList("Trash"));
        }

        //RECOVER
        //GET: Admin/Category/Recover/5
        public ActionResult Recover(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi mẫu tin thất bại");
                return RedirectToAction("Index");
            }
            //Truy vấn id
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                TempData["message"] = new XMessage("danger", "Phục hồi mẫu tin thất bại");
                return RedirectToAction("Index");
            }
            else
            {
                //chuyển đổi trạng thái status 0 -> 2
                products.Status = 2;

                //Cập nhật UpdateAt
                products.UpdateAt = DateTime.Now;

                //Cập nhật lại data
                productsDAO.Update(products);

                //Thông báo phục hồi dữ liệu thành công
                TempData["message"] = new XMessage("success", "Phục hồi mẫu tin thành công");
                return RedirectToAction("Index");
            }
        }

    }
}
