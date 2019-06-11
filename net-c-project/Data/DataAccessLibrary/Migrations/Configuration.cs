namespace PCHI.DataAccessLibrary.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using PCHI.Model.Messages;
    using PCHI.Model.Questionnaire;
    using PCHI.Model.Security;
    using PCHI.Model.Users;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The configuration class. Handles initial configuration of the database
    /// </summary>
    public sealed class Configuration : DbMigrationsConfiguration<PCHI.DataAccessLibrary.Context.MainDatabaseContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class
        /// </summary>
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = false;
        }

        /// <summary>
        /// Seeds the database with data
        /// </summary>
        /// <param name="context">The context to use</param>
        protected override void Seed(PCHI.DataAccessLibrary.Context.MainDatabaseContext context)
        {
            /*
             * This method will be called after migrating to the latest version. 
             * 
             * You can use the DbSet<T>.AddOrUpdate() helper extension method 
             * to avoid creating duplicate seed data. E.g.
             * 
             *    context.People.AddOrUpdate(
             *      p => p.FullName,
             *      new Person { FullName = "Andrew Peters" },
             *      new Person { FullName = "Brice Lambson" },
             *      new Person { FullName = "Rowan Miller" }
             *    );
             */

            {
                QuestionnaireDataExtraction[] qde = new QuestionnaireDataExtraction[]
                { 
                    new QuestionnaireDataExtraction() { QuestionnaireName = "OPSMCFirstAppointment", ItemActionId = "OPSMC.39", OptionGroupActionId = "OPSMC.39.1", OptionActionId= "OPSMC.39.1.1", TagName = "Weight" },
                    new QuestionnaireDataExtraction() { QuestionnaireName = "OPSMCFirstAppointment", ItemActionId = "OPSMC.39", OptionGroupActionId = "OPSMC.39.2", OptionActionId ="OPSMC.39.2.1", TagName = "Height" },
                    new QuestionnaireDataExtraction() { QuestionnaireName = "OPSMCFirstAppointment", ItemActionId = "OPSMC.15.1", OptionGroupActionId = "OPSMC.15.1.1", TagName = "Gender" },
                    new QuestionnaireDataExtraction() { QuestionnaireName = "OPSMCFirstAppointment", ItemActionId = "OPSMC.41", OptionGroupActionId = null, TagName = "Occupation" },
                    new QuestionnaireDataExtraction() { QuestionnaireName = "OPSMCFirstAppointment", ItemActionId = "OPSMC.45", OptionGroupActionId = "OPSMC.45.1", TagName = "Main Sport" },
                    new QuestionnaireDataExtraction() { QuestionnaireName = "OPSMCFirstAppointment", ItemActionId = "OPSMC.45", OptionGroupActionId = "OPSMC.45.2", TagName = "Seconday Sport" },
                    new QuestionnaireDataExtraction() { QuestionnaireName = "OPSMCFirstAppointment", ItemActionId = "OPSMC.46", OptionGroupActionId = "OPSMC.46.1", TagName = "Current sporting level" },
                };
                context.QuestionnaireDataExtractions.AddOrUpdate(qde);
                context.SaveChanges();
            }

            {
                Dictionary<ErrorCodes, string> errorList = new Dictionary<ErrorCodes, string>();
                errorList.Add(ErrorCodes.ERROR_DOES_NOT_EXIST, "That error code doesn't exist");
                errorList.Add(ErrorCodes.DATA_LOAD_ERROR, "There has been a problem loading the data");
                errorList.Add(ErrorCodes.LOGIN_FAILED, "Login has failed");
                errorList.Add(ErrorCodes.USER_IS_LOCKEDOUT, "You are currently locked out.");
                errorList.Add(ErrorCodes.USER_SESSION_EXPIRED, "Session has expired");
                errorList.Add(ErrorCodes.GENERAL_IDENTITY_RESULT_ERROR, "Put error of IdentityResult here");
                errorList.Add(ErrorCodes.USER_UNKNOWN, "User doesn't exist");
                errorList.Add(ErrorCodes.QUESTIONNAIRE_NOT_ASSIGNED, "No questionnaire of this name has been assigned to this user");
                errorList.Add(ErrorCodes.ANONYMOUS_QUESTIONNAIRE_CANNOT_BE_CONTINUED_ANONYMOUSLY, "A questionnaire that already has responses filled in can not be accessed anonymously");
                errorList.Add(ErrorCodes.EPISODE_DOESNT_EXIST, "The requested episode doesn't exist");
                errorList.Add(ErrorCodes.MILESTONE_DOESNT_EXIST, "The requested milestone doesn't exist");
                errorList.Add(ErrorCodes.CODE_INCORRECT, "The provided code is incorrect");
                errorList.Add(ErrorCodes.USER_ALREADY_COMPLETED_REGISTRATION, "The user has already completed registration and cannot complete it again.");
                errorList.Add(ErrorCodes.USER_IS_REQUIRED, "The user has to be specified");
                errorList.Add(ErrorCodes.NO_ACTIVE_SESSION_SET, "No session has been set as active.");
                errorList.Add(ErrorCodes.PERMISSION_DENIED, "Permission has been denied");
                errorList.Add(ErrorCodes.SECURITY_QUESTION_ANSWER_ARE_MANDATORY, "The security question and answer are mandatory and cannot be empty.");
                errorList.Add(ErrorCodes.PASSWORD_INCORRECT, "The provided password is incorrect");
                errorList.Add(ErrorCodes.REGISTRATION_NOT_COMPLETED, "You have not yet completed your registration. Please see your email for details.");
                errorList.Add(ErrorCodes.USERNAME_ALREADY_INUSE, "The username is already in use.");
                errorList.Add(ErrorCodes.USERNAME_LENGTH_EXCEEDED, "The username must be less than 450 characters.");
                errorList.Add(ErrorCodes.USERNAME_CONTAINS_ILLEGAL_CHARACTERS, "The username contains illegal characters.");
                /*
                 * 100 - 200 Generic Errors
                 * 210 - 220 User Errors
                 * 250 - 260 Questionnaire Errors                 
                 */
                var x = from e in errorList select new PCHIError(e.Key, e.Value);
                context.ErrorsMessages.AddOrUpdate(x.ToArray());

                context.SaveChanges();
            }

            {
                Dictionary<string, string> text = new Dictionary<string, string>() 
                {
                    { "Patient_Index_Top", "RePLAY allows you to prepare for upcoming appointments as well as track your health progress by completing questionnaires assigned by your practitioner. At OPSMC we really want to make you happy and healthy, so thank you for taking the time to fill this out."},
                    { "Patient_Index_Header", "Welcome back <%PatientDisplayName%/>"},
                    { "Patient_NewsFeed_1", "Mr Geoff Tymms Orthopaedic foot and ankle surgeon is now also consulting at OPSMC Geelong Campus." },
                    { "Patient_NewsFeed_2", "Dr. Pedro Chan brain surgeon is now also consulting at OPSMC Geelong Campus." },
                    { "Patient_NewsFeed_3", "You're what you eat, so don't be fast, cheap, easy or fake.<br /><br />Remember push hard and RePLAY!" },
                    {"Any_Score", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."},
                    {"Score_Better_Text", "Congratulations your score has improved. Keep it up!"},
                    {"Score_Same_Text", "No change has been found in the overall score for your condition however there may be some change in certain symptoms and/or your level of function. If this is unexpected, you may wish to make arrangements for follow-up with your practitioner if you haven’t already."},
                    {"Score_Worse_Text", "Your overall score suggests that your condition may have worsened. If this is unexpected or you don’t have arrangements for a follow-up you should consider reviewing your progress with your practitioner."},
                    {"Access_Registration_Top", "Dear <%DisplayName%/>,thanks for taking action. Please fill in the following security settings so your account can be fully activated and you can get on the way to recovery."},
                    {"UserName", "<%UserName%/>"}
                };

                context.PageTexts.AddOrUpdate(text.Select(t => new PageText() { Identifier = t.Key, Text = t.Value }).ToArray());
                context.SaveChanges();
            }

            {
                string[] roles = new string[] { "Administrator", "PatientProxy", "Telephonist", "Practitioner", "Researcher" };

                foreach (string role in roles)
                {
                    if (!context.IdentityRoles.Any(r => r.Name == role))
                        context.IdentityRoles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole(role));
                }

                context.SaveChanges();
            }

            {
                // TODO Create Roles and Permissions and assign to Users
                // context.IdentityRolePermission.AddOrUpdate(new IdentityRolePermission() { RoleId = role.Id, Permission = Permission.CREATE_USER, PermissionString = Permission.CREATE_USER.ToString() });
                {
                    IdentityRole role = context.IdentityRoles.Where(r => r.Name == "Administrator").Single();
                    context.IdentityRolePermission.AddOrUpdate(new IdentityRolePermission(role, Permission.CREATE_USER));
                    context.IdentityRolePermission.AddOrUpdate(new IdentityRolePermission(role, Permission.UPDATE_OTHER_USERS_DATA));
                    context.IdentityRolePermission.AddOrUpdate(new IdentityRolePermission(role, Permission.UPDATE_OTHER_USERS_PASSWORD));
                    context.IdentityRolePermission.AddOrUpdate(new IdentityRolePermission(role, Permission.MODIFY_QUESTIONNAIRES));
                    context.IdentityRolePermission.AddOrUpdate(new IdentityRolePermission(role, Permission.SAVE_PAGE_TEXT));
                    context.IdentityRolePermission.AddOrUpdate(new IdentityRolePermission(role, Permission.VIEW_AUDIT_TRAILS));
                }

                {
                    IdentityRole role = context.IdentityRoles.Where(r => r.Name == "Telephonist").Single();
                    context.IdentityRolePermission.AddOrUpdate(new IdentityRolePermission(role, Permission.CREATE_EPISODES));
                    context.IdentityRolePermission.AddOrUpdate(new IdentityRolePermission(role, Permission.CREATE_PATIENT));
                    context.IdentityRolePermission.AddOrUpdate(new IdentityRolePermission(role, Permission.VIEW_PATIENT_ASSIGNED_EPISODES));
                    context.IdentityRolePermission.AddOrUpdate(new IdentityRolePermission(role, Permission.VIEW_PATIENT_DATA));
                    context.IdentityRolePermission.AddOrUpdate(new IdentityRolePermission(role, Permission.UPDATE_PATIENT_DATA));
                    context.IdentityRolePermission.AddOrUpdate(new IdentityRolePermission(role, Permission.MODIFY_EPISODES));
                }

                {
                    IdentityRole role = context.IdentityRoles.Where(r => r.Name == "Practitioner").Single();
                    //context.IdentityRolePermission.AddOrUpdate(new IdentityRolePermission(role, Permission.CREATE_EPISODES));
                    //context.IdentityRolePermission.AddOrUpdate(new IdentityRolePermission(role, Permission.MODIFY_EPISODES));
                    //context.IdentityRolePermission.AddOrUpdate(new IdentityRolePermission(role, Permission.VIEW_PATIENT_ASSIGNED_EPISODES));
                    //context.IdentityRolePermission.AddOrUpdate(new IdentityRolePermission(role, Permission.VIEW_PATIENT_DATA));
                    context.IdentityRolePermission.AddOrUpdate(new IdentityRolePermission(role, Permission.UPDATE_PATIENT_DATA));
                }

                context.SaveChanges();
            }

            {
                User user = context.Users.Where(u => u.UserName == "admin").SingleOrDefault();
                if (user == null)
                {
                    AccessHandlerManager handler = new AccessHandlerManager(context);
                    user = new User() { UserName = "admin", Email = string.Empty, EmailConfirmed = true, Title = "Mr", FirstName = "Admin", LastName = "Test" };

                    Microsoft.AspNet.Identity.UserManager<User> m = new Microsoft.AspNet.Identity.UserManager<User>(handler.UserAccessHandler);
                    m.CreateAsync(user, "Welc0me!").Wait();
                }
            }

            // Add all the roles with exception of the Patient Proxy to the admin
            {
                AccessHandlerManager handler = new AccessHandlerManager(context);
                Microsoft.AspNet.Identity.UserManager<User> m = new Microsoft.AspNet.Identity.UserManager<User>(handler.UserAccessHandler);
                User u = m.FindByName("admin");

                if (u != null)
                {
                    var userRoles = m.GetRoles(u.Id).Select(r => r.ToUpper()).ToList();
                    List<string> roles = new string[] { "Administrator"/*, "Telephonist", "Practitioner", "Researcher"*/ }.ToList();

                    foreach (string role in roles)
                    {
                        if (!userRoles.Contains(role.ToUpper()))
                        {
                            IdentityResult r = m.AddToRole(u.Id, role);

                            if (!r.Succeeded)
                            {
                                string error = r.Errors.Aggregate((s1, s2) =>
                                {
                                    return s1 + Environment.NewLine + s2;
                                });

                                throw new Exception(error);
                            }
                        }
                    }
                }

                context.SaveChanges();
            }

            {
                List<TextReplacementCode> codes = new List<TextReplacementCode>();
                codes.Add(new TextReplacementCode() { ReplacementCode = "<%DisplayName%/>", ObjectKey = ReplaceableObjectKeys.Patient, ObjectVariablePath = "DisplayName", UseReplacementValue = false });
                codes.Add(new TextReplacementCode() { ReplacementCode = "<%DisplayName%/>", ObjectKey = ReplaceableObjectKeys.User, ObjectVariablePath = "DisplayName", UseReplacementValue = false });
                codes.Add(new TextReplacementCode() { ReplacementCode = "<%PatientDisplayName%/>", ObjectKey = ReplaceableObjectKeys.Patient, ObjectVariablePath = "DisplayName", UseReplacementValue = false });
                codes.Add(new TextReplacementCode() { ReplacementCode = "<%PatientTitle%/>", ObjectKey = ReplaceableObjectKeys.Patient, ObjectVariablePath = "Title", UseReplacementValue = false });
                codes.Add(new TextReplacementCode() { ReplacementCode = "<%PatientFirstName%/>", ObjectKey = ReplaceableObjectKeys.Patient, ObjectVariablePath = "FirstName", UseReplacementValue = false });
                codes.Add(new TextReplacementCode() { ReplacementCode = "<%PatientLastName%/>", ObjectKey = ReplaceableObjectKeys.Patient, ObjectVariablePath = "LastName", UseReplacementValue = false });
                codes.Add(new TextReplacementCode() { ReplacementCode = "<%PatientPhoneNumber%/>", ObjectKey = ReplaceableObjectKeys.Patient, ObjectVariablePath = "User.PhoneNumber", UseReplacementValue = false });
                codes.Add(new TextReplacementCode() { ReplacementCode = "<%PatientDateOfBirth%/>", ObjectKey = ReplaceableObjectKeys.Patient, ObjectVariablePath = "DateOfBirth", ToStringParameter = "yyyy-MM-dd", UseReplacementValue = false });
                codes.Add(new TextReplacementCode() { ReplacementCode = "<%PatientEmail%/>", ObjectKey = ReplaceableObjectKeys.Patient, ObjectVariablePath = "User.Email", UseReplacementValue = false });
                codes.Add(new TextReplacementCode() { ReplacementCode = "<%hostname%/>", ObjectKey = ReplaceableObjectKeys.UriHostName, ObjectVariablePath = string.Empty, ToStringParameter = string.Empty, UseReplacementValue = false });
                codes.Add(new TextReplacementCode() { ReplacementCode = "<%code%/>", ObjectKey = ReplaceableObjectKeys.Code, ObjectVariablePath = string.Empty, ToStringParameter = string.Empty, UseReplacementValue = false });
                codes.Add(new TextReplacementCode() { ReplacementCode = "<%UserName%/>", ObjectKey = ReplaceableObjectKeys.User, ObjectVariablePath = "UserName", ToStringParameter = string.Empty, UseReplacementValue = false });
                
                foreach (TextReplacementCode trc in codes)
                {
                    if (!context.TextReplacementCodes.Any(t => t.ReplacementCode == trc.ReplacementCode))
                    {
                        context.TextReplacementCodes.Add(trc);
                    }
                }

                context.SaveChanges();
            }

            {
                List<TextDefinition> definitions = new List<TextDefinition>()
                {
                    new TextDefinition()
                    {
                        DefinitionCode = "RegistrationEmail",
                        Text = "Dear <%DisplayName%/>, \n Thanks for booking an appointment with OPSMC. \n\n As part of your journey with us, you will be using our ground-breaking RePLAY service, which will let you prepare for your upcoming appointment as well as help your practitioner track your health progress. \n Please click on the link below, or copy/paste it to your web browser to complete your registration in RePLAY. \n\n <%hostname%/>Access/Login?CompleteRegistration=<%code%/> \n Thanks and regards, \n The OPSMC Team (http://www.opsmc.com.au/) \nPhone: 1300 859 887 – (03) 9420 4300",
                        Html = "Dear <%DisplayName%/>, \n<br /> Thanks for booking an appointment with OPSMC. \n<br />\n<br /> As part of your journey with us, you will be using our ground-breaking RePLAY service, which will let you prepare for your upcoming appointment as well as help your practitioner track your health progress. \n<br /> Please click on the link below, or copy/paste it to your web browser to complete your registration in RePLAY. \n<br />\n<br /> <a href=\"<%hostname%/>Access/CompleteRegistration?code=<%code%/>\"><%hostname%/>/Access/CompleteRegistration?code=<%code%/></a>  \n<br /> Thanks and regards, \n<br /> The OPSMC Team (<a href=\"http://www.opsmc.com.au/\">http://www.opsmc.com.au/</a>) \n<br />Phone: 1300 859 887 – (03) 9420 4300"
                    },

                    new TextDefinition()
                    {
                        DefinitionCode = "TwoStageCodeSubject",
                        Text = "OPSMC RePLAY Access Code",
                        Html = "OPSMC RePLAY Access Code"
                    },

                    new TextDefinition()
                    {
                        DefinitionCode = "TwoStageCodeBody",
                        Text = "Your security code is <%code%/>",
                        Html = "Your security code is <%code%/>"
                    },

                    new TextDefinition()
                    {
                        DefinitionCode = "NotificationStart",
                        Text = "Dear <%DisplayName%/>,\n",
                        Html = "Dear <%DisplayName%/>,\n<br>"
                    },

                    new TextDefinition()
                    {
                        DefinitionCode = "NotificationEnd",
                        Text = "Thanks and regards,\nThe OPSMC Team (link to OPSMC website)\nPhone: 1300 859 887 – (03) 9420 4300",
                        Html = "Thanks and regards,<br/>The OPSMC Team (link to OPSMC website)\n<br/>Phone: 1300 859 887 – (03) 9420 4300"
                    },

                    new TextDefinition()
                    {
                        DefinitionCode = "RegistrationComplete",
                        Text = "Thank you for completing your registration here at the RePLAY service from OPSMC. You can now login and start on the path to recovery.",
                        Html = "Thank you for completing your registration here at the RePLAY service from OPSMC. You can now login and start on the path to recovery."
                    },

                    new TextDefinition()
                    {
                        DefinitionCode = "NewQuestionnaire",
                        Text = "Your practitioner needs to get some information from you. Please follow the link below, or copy/paste it into your web browser, and you will be taken to your pending questionnaires page.\n<%hostname%/>Utility/MyQuestionnaires",
                        Html = "Your practitioner needs to get some information from you. Please follow the link below, or copy/paste it into your web browser, and you will be taken to your pending questionnaires page.<br /><a href=\"<%hostname%/>Utility/MyQuestionnaires\"><%hostname%/>Utility/MyQuestionnaires</a>"
                    },

                    new TextDefinition()
                    {
                        DefinitionCode = "PasswordChanged",
                        Text = "Your password has recently been changed successfuly. \n If you have not changed your password, please contact OPSMC immediately for support as your account may have been compromised.\n",
                        Html = "Your password has recently been changed successfuly. <br/> If you have not changed your password, please contact OPSMC immediately for support as your account may have been compromised.<br/>",
                    },

                    new TextDefinition()
                    {
                        DefinitionCode = "UserDetailsChanged",
                        Text = "Your user details have recently been updated successfuly. \n If you have not updated your user details, please contact OPSMC immediately for support as your account may have been compromised.\n",
                        Html = "Your user details have recently been updated successfuly. <br/> If you have not updated your user details, please contact OPSMC immediately for support as your account may have been compromised.<br/>",
                    },

                    new TextDefinition()
                    {
                        DefinitionCode = "PasswordReset",
                        Text = "A password reset request has been received for your account. \n In order to reset your password, you will need to please follow the link below to reset your password: \n <%hostname%/>Access/ResetPassword?token=<%code%/>  \n\n ",
                        Html = "A password reset request has been received for your account. <br/> In order to reset your password, you will need to please follow the link below to reset your password: <br/><a href=\"<%hostname%/>Access/ResetPassword?token=<%code%/>\"><%hostname%/>Access/ResetPassword?token=<%code%/> </a> <br/> ",
                    }
                };
                foreach (TextDefinition t in definitions)
                {
                    context.TextDefinitions.AddOrUpdate(t);
                }

                context.SaveChanges();
            }
        }

        /// <summary>
        /// write a message to the Package Manager Console
        /// </summary>
        public void Debug(PCHI.DataAccessLibrary.Context.MainDatabaseContext context, string s, params object[] args)
        {
            var fullString = string.Format(s, args).Replace("'", "''");
            context.Database.ExecuteSqlCommand(string.Format("print '{0}'", fullString));
            throw new Exception(s);
        }
    }
}
