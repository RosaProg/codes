using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteSupportLibrary.Models;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Service;

namespace WebsiteSupportLibrary.ControllerHelpers
{
    public class AccountControllerHelper
    {
        public QuestionnaireModel CreateCompleteRegistationQuestionnaire()
        {
            QuestionnaireModel model = new QuestionnaireModel();
            model.IsPro = false;
            model.CanSavePartial = false;
            model.ShowProgressBar = false;


            model.Items.Add(new QuestionnaireItem()
            {
                Question = new Question()
                {
                    HtmlContent = @"Hello and welcome to RePLAY, a groundbreaking new service that allows you to track your health progress and prepare for your upcoming appointments at OPSMC. 
                    Right now you are in the process of completing your account registration, during which you will choose a secret password and select other important security and privacy options. 
                    Here at OPSMC we take your privacy very seriously. We’ve put a lot of time and effort into keeping your information secure and you can see just how we do this in our strict privacy and confidentiality policy. 
                    Are you ready to start?"
                },
                ResponsePanel = new ResponsePanel() { HtmlContent = "Yes, let's start!", CanSkip = false },
                Answer = new Answer() { HtmlTemplate = "Yes, let's start!" },
                AnsweredStatus = ItemAnsweredStatus.NotAnswered.ToString(),
                Status = ItemStatus.Current.ToString(),
                ItemNames = new List<string>()
            });

            model.Items.Add(new QuestionnaireItem()
            {
                Question = new Question() { HtmlContent = "The best way to keep your information as secure as Fort Knox is to choose a strong password. This means your password will need to have at least: 8 characters, a capital letter, a number and, a symbol. Please type in a password that fits these criteria, and make sure it’s one that you’ll remember! Please fill it in twice to ensure no errors were made." },
                ResponsePanel = new ResponsePanel() { HtmlContent = "<table><tr><td>Password:</td><td><input name=\"Password\" type=\"password\" /></td></tr><tr><td>Repeat password</td><td><input name=\"passwordrepeat\" type=\"password\" /></td></tr></table>", CanSkip = false },
                Answer = new Answer() { HtmlTemplate = "My password is *****" },
                AnsweredStatus = ItemAnsweredStatus.NotAnswered.ToString(),
                Status = ItemStatus.Future.ToString(),
                ItemNames = new List<string>(new string[] { "Password" })
            });

            model.Items.Add(new QuestionnaireItem()
            {
                Question = new Question() { HtmlContent = "With this password your access to the system is secure enough. However, if you want to be extra cautious you could opt to have a second step in the login process whereby a security code will be sent to your mobile phone or email every time you want to login. What level of secure access would you like to have?" },
                ResponsePanel = new ResponsePanel() { HtmlContent = "", CanSkip = false },
                Answer = new Answer() { HtmlTemplate = "%Value%" },
                AnsweredStatus = ItemAnsweredStatus.NotAnswered.ToString(),
                Status = ItemStatus.Future.ToString(),
                ItemNames = new List<string>(new string[] { "Provider" })
            });


            ServiceDetailsClient client = new ServiceDetailsClient();
            var result = client.GetTwoStageAuthenticationProviders();
            int currentItem = model.Items.Count - 1;
            model.Items[currentItem].ResponsePanel.HtmlContent = "<input type=\"radio\" name=\"Provider\" value=\"None\" checked>User name and password.";
            model.Items[currentItem].PossibleAnswers.Add(new PossibleAnswers() { Name = "Provider", Value = "None", Action = "GOTO SecurityQuestion", AnswerText = "User name and password" });
            foreach (var provider in result.Strings)
            {
                model.Items[currentItem].ResponsePanel.HtmlContent += "<br /><input type=\"radio\" name=\"Provider\" value=\"" + provider + "\">" + "User name, password and " + provider;
                model.Items[currentItem].PossibleAnswers.Add(new PossibleAnswers() { Name = "Provider", AnswerText = "User name, password and " + provider, Value = provider });
            }

            /*
            model.Items.Add(new QuestionnaireItem()
            {
                Question = new Question() { HtmlContent = "OK, can you fill in your mobile number below?" },
                ResponsePanel = new ResponsePanel() { HtmlContent = "<input type=\"text\" name=\"Mobile\">", CanSkip = false },
                Answer = new Answer() { HtmlTemplate = string.Empty },
                AnsweredStatus = ItemAnsweredStatus.NotAnswered.ToString(),
                Status = ItemStatus.Future.ToString(),
                ItemNames = new List<string>(new string[] { "TwoStageCode" })
            });
            */

            model.Items.Add(new QuestionnaireItem()
            {
                Question = new Question() { HtmlContent = "A secure code has been sent to your e-mail. Did you receive it? " },
                ResponsePanel = new ResponsePanel() { HtmlContent = "<input type=\"radio\" value=\"yes\" name=\"codeCheckRadio\"/> Yes, the security code is <input type=\"text\" name=\"TwoStageCode\"> <input type=\"radio\" value=\"No\" name=\"codeCheckRadio\"> No, please send it again ", CanSkip = false },
                Answer = new Answer() { HtmlTemplate = string.Empty },
                AnsweredStatus = ItemAnsweredStatus.NotAnswered.ToString(),
                Status = ItemStatus.Future.ToString(),
                ItemNames = new List<string>(new string[] { "codeCheckRadio", "TwoStageCode" }),
            });

            model.Items.Add(new QuestionnaireItem()
            {
                ActionId = "SecurityQuestion",
                Question = new Question() { HtmlContent = "In case your password get's lost, you will need a security question and answer to reset your password. <br/> Please fill in your security question of choice and what the answer should be. The answer is NOT case sensitive." },
                ResponsePanel = new ResponsePanel() { HtmlContent = "My Security Question: <input type=\"text\" name=\"TextBox.SecurityQuestion\"/> <br /> My Security Answer: <input type=\"text\" name=\"TextBox.SecurityAnswer\">", CanSkip = false },
                Answer = new Answer() { HtmlTemplate = "My Security Question is: %Value% <br /> My Answer is: ****" },
                AnsweredStatus = ItemAnsweredStatus.NotAnswered.ToString(),
                Status = ItemStatus.Future.ToString(),
                ItemNames = new List<string>(new string[] { "TextBox.SecurityQuestion", "TextBox.SecurityAnswer" }),
            });

            model.Items.Add(new QuestionnaireItem()
            {
                ActionId = "Privacy",
                Question = new Question() { HtmlContent = "Here at OPSMC we take your privacy and confidentiality very seriously and make every effort to keep your information secure. Please, take some time to read our policies, terms and conditions." },
                ResponsePanel = new ResponsePanel() { HtmlContent = "<input type=\"checkbox\" name=\"PandCPolicy\" value=\"pandc\" /> I have read and accept OPSMC’S Privacy and confidentiality policy <br /> <input type=\"checkbox\" name=\"TandCPolicy\" value=\"tandc\" /> I have read and accept the RePLAY terms and conditions", CanSkip = false },
                Answer = new Answer() { HtmlTemplate = "I have read and understood both the Privacy and Confidentiality policy as well as the Terms and Conditions of RePLAY" },
                AnsweredStatus = ItemAnsweredStatus.NotAnswered.ToString(),
                Status = ItemStatus.Future.ToString(),
                ItemNames = new List<string>(new string[] { "PandCPolicy", "TandCPolicy" })
            });

            model.Items.Add(new QuestionnaireItem()
            {
                Question = new Question() { HtmlContent = "Please tell us how you would like us to share your information within OPSMC." },
                ResponsePanel = new ResponsePanel() { HtmlContent = "<table><tr><td>For OPSMC’s quality improvement</td><td><input type=\"radio\" name=\"Radio.quality\" value=\"open\" > Open </td><td><input type=\"radio\" name=\"Radio.quality\" value=\"Confidentially\" > Confidentially </td></tr><tr><td>For OPSMC’s administrative purposes: </td><td><input type=\"radio\" name=\"Radio.administrative\" value=\"open\" > Open</td><td> <input type=\"radio\" name=\"Radio.administrative\" value=\"Confidentially\" > Confidentially </td></tr><tr><td>For research purposes</td><td> <input type=\"radio\" name=\"Radio.research\" value=\"open\" > Open</td><td> <input type=\"radio\" name=\"Radio.research\" value=\"Confidentially\" > Confidentially </td></tr></table>", CanSkip = false },
                Answer = new Answer() { HtmlTemplate = "%Value% <br/> %Value% <br/> %Value%" },
                AnsweredStatus = ItemAnsweredStatus.NotAnswered.ToString(),
                Status = ItemStatus.Future.ToString(),
                ItemNames = new List<string>(new string[] { "Radio.quality", "Radio.administrative", "Radio.research" })
            });

            currentItem = model.Items.Count - 1;
            model.Items[currentItem].PossibleAnswers.Add(new PossibleAnswers() { Name = "Radio.quality", AnswerText = "Your quality improvement suggestions can be shared openly.", Value = "open" });
            model.Items[currentItem].PossibleAnswers.Add(new PossibleAnswers() { Name = "Radio.quality", AnswerText = "Your quality improvement suggestions has to be kept confidential.", Value = "Confidentially" });

            model.Items[currentItem].PossibleAnswers.Add(new PossibleAnswers() { Name = "Radio.administrative", AnswerText = "Your administrative information can be shared openly.", Value = "open" });
            model.Items[currentItem].PossibleAnswers.Add(new PossibleAnswers() { Name = "Radio.administrative", AnswerText = "Your administrative information has to be kept confidential.", Value = "Confidentially" });

            model.Items[currentItem].PossibleAnswers.Add(new PossibleAnswers() { Name = "Radio.research", AnswerText = "We can use your personal data research purposes.", Value = "open" });
            model.Items[currentItem].PossibleAnswers.Add(new PossibleAnswers() { Name = "Radio.research", AnswerText = "Your personal data has to be kept confidential for research purposes.", Value = "Confidentially" });

            model.Items.Add(new QuestionnaireItem()
            {
                ActionId = "<end>",
                Question = new Question() { HtmlContent = "Thanks! As soon as you press the submit button, you will be redirected to the login page where you can use your email address and the password you just created to log in to RePLAY. You will shortly receive and email confirming the completion of your registration and pointing you to a questionnaire you need to respond before you attend your appointment. See you soon!" },
                Status = ItemStatus.Future.ToString(),
                Answer = new Answer() { HtmlTemplate = string.Empty },
                AnsweredStatus = ItemAnsweredStatus.NotAnswered.ToString(),
                ResponsePanel = new ResponsePanel()
                {
                    CanSkip = false
                }
            });

            return model;

        }
    }
}
