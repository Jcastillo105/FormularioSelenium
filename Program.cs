using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumFormFilling
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configurar ChromeDriver especificando la ruta del ejecutable
            var chromeDriverService = ChromeDriverService.CreateDefaultService(AppDomain.CurrentDomain.BaseDirectory);
            var chromeOptions = new ChromeOptions();
            IWebDriver driver = new ChromeDriver(chromeDriverService, chromeOptions);

            try
            {
                // Navegar a la página del formulario
                string url = "file:///C:/Users/josee/source/repos/FormularioSelenium/Properties/index.html";
                driver.Navigate().GoToUrl(url);
                Console.WriteLine($"Navegando a {url}");

                // Espera explícita para asegurar que los elementos estén presentes
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

                // Localizar y rellenar los elementos del formulario
                RellenarFormulario(wait, driver);

                // Enviar el formulario
                EnviarFormulario(wait, driver);

                // Obtener el nombre del usuario
                string nombreUsuario = ObtenerNombreUsuario(driver).Replace(" ", "_");

                // Guardar los datos en un archivo de texto en la ubicación especificada
                GuardarDatos(nombreUsuario, driver);

                // Verificar la redirección a submit.html
                wait.Until(ExpectedConditions.UrlContains("submit.html"));
                Console.WriteLine("Formulario enviado y redirigido a submit.html correctamente.");
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine($"NoSuchElementException capturada: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Se produjo un error: {ex.Message}");
            }
            finally
            {
                // Cerrar el navegador
                driver.Quit();
            }
        }

        static void RellenarFormulario(WebDriverWait wait, IWebDriver driver)
        {
            try
            {
                Console.WriteLine("Localizando y rellenando el campo de nombre...");
                var nombreElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("nombre")));
                nombreElement.SendKeys("Juan Pérez");

                Console.WriteLine("Localizando y seleccionando el género masculino...");
                var masculinoElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("masculino")));
                masculinoElement.Click();

                Console.WriteLine("Localizando y rellenando el campo de email...");
                var emailElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("email")));
                emailElement.SendKeys("juan.perez@example.com");

                Console.WriteLine("Localizando y rellenando el campo de fecha...");
                var fechaElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("fecha")));
                fechaElement.SendKeys("1990-01-01");

                Console.WriteLine("Localizando y seleccionando el municipio...");
                var municipioElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("municipio")));
                municipioElement.SendKeys("Santiago");

                Console.WriteLine("Localizando y seleccionando la edad...");
                var edadElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("edad")));
                edadElement.SendKeys("adulto");

                Console.WriteLine("Localizando y seleccionando los paisajes...");
                var paisajesElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("paisajes")));
                paisajesElement.Click();

                Console.WriteLine("Localizando y seleccionando la gente...");
                var genteElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("gente")));
                genteElement.Click();

                Console.WriteLine("Localizando y seleccionando el tráfico...");
                var traficoElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("trafico")));
                traficoElement.Click();

                Console.WriteLine("Localizando y seleccionando la contaminación...");
                var contaminacionElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("contaminacion")));
                contaminacionElement.Click();

                Console.WriteLine("Localizando y rellenando el campo de texto...");
                var textElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("text")));
                textElement.SendKeys("Muy satisfecho con el municipio en general.");
            }
            catch (StaleElementReferenceException ex)
            {
                Console.WriteLine($"StaleElementReferenceException capturada. Reintentando rellenar el formulario... {ex.Message}");
                RellenarFormulario(wait, driver);
            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine($"WebDriverTimeoutException capturada. Reintentando rellenar el formulario... {ex.Message}");
                RellenarFormulario(wait, driver);
            }
        }

        static void EnviarFormulario(WebDriverWait wait, IWebDriver driver)
        {
            try
            {
                Console.WriteLine("Localizando y haciendo clic en el botón de enviar...");
                var submitButton = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("input[type='submit']")));
                submitButton.Click();
            }
            catch (StaleElementReferenceException ex)
            {
                Console.WriteLine($"StaleElementReferenceException capturada al enviar. Reintentando... {ex.Message}");
                EnviarFormulario(wait, driver);
            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine($"WebDriverTimeoutException capturada al enviar. Reintentando... {ex.Message}");
                EnviarFormulario(wait, driver);
            }
        }

        static string ObtenerNombreUsuario(IWebDriver driver)
        {
            try
            {
                Console.WriteLine("Obteniendo el nombre del usuario...");
                var nombreElement = driver.FindElement(By.Id("nombre"));
                return nombreElement.GetAttribute("value");
            }
            catch (StaleElementReferenceException ex)
            {
                Console.WriteLine($"StaleElementReferenceException capturada al obtener el nombre. Reintentando... {ex.Message}");
                return ObtenerNombreUsuario(driver);
            }
        }

        static void GuardarDatos(string nombreUsuario, IWebDriver driver)
        {
            string folderPath = @"C:\Users\josee\source\repos\FormularioSelenium\Formularios Completos";
            Directory.CreateDirectory(folderPath); // Crear la carpeta si no existe

            string filePath = Path.Combine(folderPath, $"{nombreUsuario}.txt");

            try
            {
                Console.WriteLine("Guardando los datos en el archivo...");
                var nombreElement = driver.FindElement(By.Id("nombre"));
                var formData = new string[]
                {
                    $"Nombre: {nombreElement.GetAttribute("value")}",
                    "Género: Masculino",
                    "Email: juan.perez@example.com",
                    "Fecha de Nacimiento: 1990-01-01",
                    "Municipio: Santiago",
                    "Edad: Adulto",
                    "Gustos: Paisajes, Gente",
                    "No Gustos: Tráfico, Contaminación",
                    "Opinión: Muy satisfecho con el municipio en general."
                };

                File.WriteAllLines(filePath, formData);

                // Verificar que el archivo se haya creado
                if (File.Exists(filePath))
                {
                    Console.WriteLine($"Formulario enviado y datos guardados en {filePath}");
                }
                else
                {
                    Console.WriteLine($"Error: El archivo {filePath} no se pudo crear.");
                }
            }
            catch (StaleElementReferenceException ex)
            {
                Console.WriteLine($"StaleElementReferenceException capturada al guardar datos. Reintentando... {ex.Message}");
                GuardarDatos(nombreUsuario, driver);
            }
        }
    }
}
