using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Register.Data;
using Register.Models;

namespace Register.Controllers
{
    public class LoginController : Controller
    {
        public readonly InfoContext _context;

        public LoginController( InfoContext infoContext)
        {  
            _context = infoContext;
        }

        public async Task<IActionResult> Main()
        {                
            var user = _context.Info.FirstOrDefault(m => m.isLogin == true);
            if (user != null)
            {
                ViewBag.index = user.Id;
                ViewBag.UserId = user.UserId;
                return View(await _context.Info.ToListAsync());
            }
            return View(await _context.Info.ToListAsync());
        }

        public IActionResult LoginScreen()
        {
            var login = _context.Info.FirstOrDefault(m => m.isLogin == true);
            if (login != null)
            {
                ViewBag.UserId = login.UserId;
                return View();
            }
            return View();  
        }

        // POST: Login/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginScreenAsync([Bind("UserId,UserPassword")] Info info,string submit)
        {  
            if (info.UserId == null || info.UserPassword == null)
            {
                //id pw  null
                ViewBag.nul = false;
                return View();
            }
            else
            {
                info.UserPassword = ComputeSha256Hash(info.UserPassword);
                var temp = _context.Info.FirstOrDefault(e => e.UserId == info.UserId);
                if (temp == null)
                {
                    ViewBag.PwCheck = false;
                    return View(info);
                }
                else
                {
                    if (temp.UserPassword == info.UserPassword)
                    {
                        var user = _context.Info.FirstOrDefault(m => m.UserId == info.UserId);
                        user.isLogin = true;
                        _context.Info.Update(user);
                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Main));
                    }
                    else
                    {
                        ViewBag.PwCheck = false;
                        return View(info);
                    }
                }
            }
        }

        public IActionResult Create() {
            var login = _context.Info.FirstOrDefault(m => m.isLogin == true);
            if (login != null)
            {
                ViewBag.UserId = login.UserId;
                return View();
            }
            return View();
        }

        // POST: Login/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,UserName,UserPassword,ConfirmPassword")] Info info)
        {
            if (info.UserId == null || info.UserPassword == null || info.UserName == null||info.ConfirmPassword == null)
            {
                    //id pw name null
                    ViewBag.nul = false;
                    return View();            
            }
            else
            {
                if (info.UserPassword == info.ConfirmPassword)
                {
                    var z = _context.Info.FirstOrDefault(m => m.UserId == info.UserId);
                    if (z != null)
                    {
                        //id 중복
                        ViewBag.duplicate = false;
                        return View();
                    }
                    else
                    {                     
                            info.UserPassword = ComputeSha256Hash(info.UserPassword);
                            info.ConfirmPassword = ComputeSha256Hash(info.ConfirmPassword);                 
                            _context.Add(info);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(LoginScreen));                      
                    }
                }
            }
            return View(info);
        }

        // GET: Infoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var info = await _context.Info.FindAsync(id);
            if (info == null)
            {
                return NotFound();
            }
            return View(info);
        }

        // POST: Infoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,UserName")] Info info)
        {          
            if (id != info.Id)
            {
                return NotFound();
            }
            if (info.UserId == null || info.UserName == null)
            {       
                ViewBag.nul = false;
                return View(info);
            }
                var data = await _context.Info.FindAsync(id);
                var ExistUser =  _context.Info.FirstOrDefault(m => m.UserId == info.UserId && m.Id != info.Id);
                if (ExistUser != null)
                {
                    ViewBag.ExiUser = ExistUser.UserId;
                    ViewBag.exist = false;
                    return View(info);
                }
               else
                {
                    data.UserId = info.UserId;
                    data.UserName = info.UserName;
                    _context.Update(data);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Main));
                }
        }

        public string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var user=_context.Info.FirstOrDefault(m => m.isLogin == true);
            user.isLogin = false;
            _context.Info.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("LoginScreen", "Login");
        }

        public async Task<IActionResult> Checking(int? id)
        {
            var data = await _context.Info.FindAsync(id);
            ViewBag.UserId = data.UserId;
            if (data == null)
            {
                return NotFound();
            }
            return View();
        }

        // GET: Infoes/Checking/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checking(int id, [Bind("Id,UserPassword")] Info info)
        {
            if (id != info.Id)
            {
                return NotFound();
            }
            if (info.UserPassword == null)
            {
                ViewBag.WrongPW = false;
                return View();
            }
            else
            { 
                    var data = _context.Info.FirstOrDefault(m => m.Id == id);
                    ViewBag.UserId = data.UserId;
                    info.UserPassword = ComputeSha256Hash(info.UserPassword);
                    if (info.UserPassword == data.UserPassword)
                    {
                        return RedirectToAction(nameof(ChangePW), new { id });
                    }
                    else
                    {
                        ViewBag.wrongPW = false;
                        return View();
                    }
            }       
        }

        public async Task<IActionResult> ChangePW(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var data = await _context.Info.FindAsync(id);
            ViewBag.UserId = data.UserId;
            if (data == null)
            {
                return NotFound();
            }
            return View();
        }

        // GET: Infoes/Checking/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePW(int id, [Bind("Id,UserPassword,ConfirmPassword")] Info info)
        {           
            if (id != info.Id)
            {
                return NotFound();
            }         
            if (ModelState.IsValid)
            {
                var data = await _context.Info.FindAsync(id);
                if (info.Id == data.Id &&(info.UserPassword==info.ConfirmPassword))
                {
                    data.UserPassword = ComputeSha256Hash(info.UserPassword);
                    data.ConfirmPassword = ComputeSha256Hash(info.ConfirmPassword);
                    data.isLogin = true;
                    _context.Update(data);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Main));
                }
            }
            else
            {
                return View();
            }
            return RedirectToAction(nameof(Main));
        }
      
        public IActionResult idcheck()
        { 
            return View();
        }
        // POST: Infoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> idcheck([Bind("Id,UserId,UserPassword")] Info info)
        {       
                if(info.UserId ==null ||info.UserPassword==null)
                {                   
                    return View();
                }
                var data = _context.Info.FirstOrDefault(m => m.UserId == info.UserId);

                if (data == null  )
                {
                    ViewBag.nul = false;
                    return View(info);
                }
                else
                {
                    info.UserPassword = ComputeSha256Hash(info.UserPassword);
                    if (data.UserId == info.UserId && data.UserPassword == info.UserPassword)
                    {

                        return RedirectToAction(nameof(ChangePW), new { data.Id });
                    }
                    else
                    {
                        ViewBag.nul = false;
                        return View(info);
                    }
                }
        }
    }
}
