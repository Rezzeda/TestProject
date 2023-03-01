using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace project_1
{
    [Authorize(Roles = "viewer, admin")]
    public class LogController : Controller
    {

        public ActionResult Index()
        {
            List<LogModel> logs = new List<LogModel>();

            return View(logs);
        }

        private void Log(string message, TraceEventType level)
        {
            string LogFilePath = "/Users/pro/Desktop/project_1/Logs/MyLog.log"; // путь к лог-файлу
            // запись сообщения в лог-файл
            using (StreamWriter sw = new StreamWriter(LogFilePath, true))
            {
                sw.WriteLine($"{DateTime.Now}: {level}: {message}");
            }

            // запись сообщения в консоль
            Console.WriteLine($"{DateTime.Now}: {level}: {message}");

        }
    }

}

