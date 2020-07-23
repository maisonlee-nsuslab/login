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
            var user = _context.Info.FirstOrDefault(m => m.login == true);
            if (user != null)
            { 
                ViewBag.userId = user.userId;
                return View(await _context.Info.ToListAsync());
            }
            return View(await _context.Info.ToListAsync());
        }



        public IActionResult LoginScreen()
        {
            var login = _context.Info.FirstOrDefault(m => m.login == true);
            if (login != null)
            {
                ViewBag.userId = login.userId;
                return View();
            }
            return View();
         
        }

        // POST: Login/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginScreenAsync([Bind("userId,userPassword")] Info info,string submit)
        {
           
            if (info.userId == null || info.userPassword == null)
            {
                //id pw  null
                ViewBag.nul = false;
                return View();
            }
            else
            {
                info.userPassword = ComputeSha256Hash(info.userPassword);
                var temp = _context.Info.FirstOrDefault(e => e.userId == info.userId);
                if (temp == null)
                {
                    ViewBag.pwCheck = false;
                    return View(info);
                }
                else
                {
                    if (temp.userPassword == info.userPassword)
                    {
                        var user = _context.Info.FirstOrDefault(m => m.userId == info.userId);
                        user.login = true;
                        _context.Info.Update(user);
                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Main));
                    }
                    else
                    {
                        ViewBag.pwCheck = false;
                        return View(info);
                    }
                }
            }
  
        }

        public IActionResult Create() {


            var login = _context.Info.FirstOrDefault(m => m.login == true);
            if (login != null)
            {
                ViewBag.userId = login.userId;
                return View();
            }

            return View();
        }
        // POST: Login/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("userId,userName,userPassword,ConfirmPassword,Id")] Info info)
        {


            if (info.userId == null || info.userPassword == null || info.userName == null||info.ConfirmPassword==null)
            {
                    //id pw name null
                    ViewBag.nul = false;
                    return View();
                
            }
            else
            {
                if (info.userPassword == info.ConfirmPassword)
                {
                    var z = _context.Info.FirstOrDefault(m => m.userId == info.userId);
                    if (z != null)
                    {
                        //id 중복
                        ViewBag.duplicate = false;
                        return View();
                    }
                    else
                    {                     
                            info.userPassword = ComputeSha256Hash(info.userPassword);
                            info.ConfirmPassword = ComputeSha256Hash(info.userPassword);                 
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,userId,userName")] Info info)
        {
           
            if (id != info.Id)
            {
                return NotFound();
            }
            if (info.userId == null || info.userName == null)
            {       
                ViewBag.nul = false;
                return View(info);
            }
            if (ModelState.IsValid)
            {

                var data = await _context.Info.FindAsync(id);
                var existUser =  _context.Info.FirstOrDefault(m => m.userId == info.userId && m.Id != info.Id);

                if (existUser != null)
                {
                    ViewBag.exiUser = existUser.userId;
                    ViewBag.exist = false;
                    return View(info);
                }

               else
                {
                    data.userId = info.userId;
                    data.userName = info.userName;
                    _context.Update(data);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Main));
                }
            }
            else
            {
                return View();
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
            var user=_context.Info.FirstOrDefault(m => m.login == true);
            user.login = false;
            _context.Info.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("LoginScreen", "Login");
        }
        public async Task<IActionResult> Checking(int? id)
        {
        
            var data = await _context.Info.FindAsync(id);
            ViewBag.userId = data.userId;
            if (data == null)
            {
                return NotFound();
            }
            return View();
        }

        // GET: Infoes/Checking/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checking(int id, [Bind("Id,userPassword")] Info info)
        {
            if (id != info.Id)
            {
                return NotFound();
            }
            if (info.userPassword == null)
            {
                ViewBag.wrongPW = false;
                return View();
            }
            else
            { 
                    var data = _context.Info.FirstOrDefault(m => m.Id == id);
                    ViewBag.userId = data.userId;
                    info.userPassword = ComputeSha256Hash(info.userPassword);
                    if (info.userPassword == data.userPassword)
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
            ViewBag.userId = data.userId;
            if (data == null)
            {
                return NotFound();
            }
            return View();
        }
        // GET: Infoes/Checking/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePW(int id, [Bind("Id,userPassword,ConfirmPassword")] Info info)
        {
            

            if (id != info.Id)
            {
                return NotFound();
            }
          
            if (ModelState.IsValid)
            {

                var data = await _context.Info.FindAsync(id);

                if (info.Id == data.Id &&(info.userPassword==info.ConfirmPassword))
                {
                    data.userPassword = ComputeSha256Hash(info.userPassword);
                    data.ConfirmPassword = ComputeSha256Hash(info.ConfirmPassword);
                    data.login = true;
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
        public async Task<IActionResult> idcheck([Bind("Id,userId,userPassword")] Info info)
        {
       
                if(info.userId ==null ||info.userPassword==null)
                {                   
                    return View();
                }
                var data = _context.Info.FirstOrDefault(m => m.userId == info.userId);

                if (data == null  )
                {
                    ViewBag.nul = false;
                    return View(info);
                }
                else
                {
                    info.userPassword = ComputeSha256Hash(info.userPassword);
                    if (data.userId == info.userId && data.userPassword == info.userPassword)
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
