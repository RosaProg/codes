using PCHI.Model.Questionnaire;
using PCHI.Model.Questionnaire.Styling.Definition.Elements;
using PCHI.Model.Questionnaire.Styling.Definition.ItemGroupOptions;
using PCHI.Model.Questionnaire.Styling.Definition.Sections;
using PCHI.Model.Questionnaire.Styling.Presentation;
using PCHI.Model.Questionnaire.Instructions;
using PCHI.Model.Questionnaire.Pro;
using PCHI.Model.Questionnaire.Response;
using PCHI.Model.Questionnaire.Survey;
using PCHI.Model.Tag;
using PCHI.Model.Users;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Model;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Questionnaire;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using DSPrima.WcfUserSession.Model;

namespace WCFTest
{
	public class Program
	{
		private UserClient userClient;
		private ProClient proClient;
		private QuestionnaireClient questionnaireClient;
		private QuestionnaireFormatClient questionnaireFormatClient;
		private PatientEpisodeClient userQuestionnaireClient;
		private PatientClient patientClient;

		#region Helper Functions
		public class ValuesOption
		{
			public string Text;
			public string Action;
			private string mappingId;
			public string ActionId 
			{
				get
				{
					return string.IsNullOrWhiteSpace(this.mappingId)? this.Text:this.mappingId;
				}
				set
				{
					this.mappingId = value;
				}
			}
		}

		public class MyDictionary : Dictionary<int, ValuesOption>
		{
			public void Add(int key, string text, string action)
			{
				ValuesOption val = new ValuesOption();
				val.Text = text;
				val.Action = action;
				this.Add(key, val);
			}
		}

		private QuestionnaireText AddTextToSection(QuestionnaireSection section, string actionId, string text, Platform platform, Instance instance)
		{
			QuestionnaireText txt = new QuestionnaireText()
			{
				ActionId = actionId,
				OrderInSection = section.Elements.Count + 1,
				Section = section
			};

			if (section.Elements.Any(e => e.ActionId == actionId))
			{
				txt = (QuestionnaireText)section.Elements.Where(e => e.ActionId == actionId).Single();
			}
			txt.TextVersions.Add(new QuestionnaireElementTextVersion() { SupportedInstances = instance, SupportedPlatforms = platform, Text = text });

			section.Elements.Add(txt);

			return txt;
		}

		private QuestionnaireItem AddItemToSection(QuestionnaireSection section, string actionId, string displayId, string text, Platform platform, Instance instance)
		{
			QuestionnaireItem item = new QuestionnaireItem()
			{
				ActionId = actionId,
				DisplayId = displayId,
				OrderInSection = section.Elements.Count + 1,
				Section = section,
			};

			if (section.Elements.Any(e => e.ActionId == actionId))
			{
				item = (QuestionnaireItem)section.Elements.Where(e => e.ActionId == actionId).Single();
			}

			item.TextVersions.Add(new QuestionnaireElementTextVersion() { SupportedInstances = instance, SupportedPlatforms = platform, Text = text });
			section.Elements.Add(item);

			return item;
		}

		private QuestionnaireItemOptionGroup AddOptionGroupToItem(QuestionnaireItem item, MyDictionary options, int rangeStep, QuestionnaireResponseType type, string optionGroupText = "", string defaultValue = null, string ActionId = null)
		{
			QuestionnaireItemOptionGroup qog = new QuestionnaireItemOptionGroup();
			qog.OrderInItem = item.OptionGroups.Count + 1;
			qog.RangeStep = rangeStep;
			qog.ResponseType = type;
			qog.DefaultValue = defaultValue;
			qog.TextVersions.Add(new QuestionnaireItemOptionGroupTextVersion() { Text = optionGroupText, SupportedPlatforms = Platform.Chat | Platform.Classic | Platform.Mobile, SupportedInstances = Instance.Baseline | Instance.Followup });
			foreach (int v in options.Keys)
			{
				qog.Options.Add(new QuestionnaireItemOption() { Text = options[v].Text, Value = v, Group = qog, Action = options[v].Action, ActionId = options[v].ActionId });
			}

			qog.Item = item;

			item.OptionGroups.Add(qog);

			return qog;
		}

		private void addBodyControl(QuestionnaireSection section, QuestionnaireItem qi, string action = "")
		{
			MyDictionary options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "headFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "headFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "headFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "headFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "headFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "headFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "neckFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "neckFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "neckFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "neckFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "neckFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "neckFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightShoulderFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightShoulderFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightShoulderFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightShoulderFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightShoulderFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightShoulderFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftShoulderFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftShoulderFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftShoulderFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftShoulderFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftShoulderFront.Ache" } } }; 
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftShoulderFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "chestFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "chestFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "chestFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "chestFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "chestFront.Ache" } } }; 
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "chestFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightArmFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightArmFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightArmFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightArmFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightArmFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightArmFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftArmFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftArmFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftArmFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftArmFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftArmFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftArmFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightElbowFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightElbowFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightElbowFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightElbowFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightElbowFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightElbowFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftElbowFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftElbowFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftElbowFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftElbowFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftElbowFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftElbowFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightForearmFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightForearmFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightForearmFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightForearmFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightForearmFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightForearmFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftForearmFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftForearmFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftForearmFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftForearmFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftForearmFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftForearmFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightWristFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightWristFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightWristFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightWristFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightWristFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightWristFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftWristFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftWristFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftWristFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftWristFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftWristFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftWristFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightHandFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightHandFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightHandFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightHandFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightHandFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightHandFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftHandFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftHandFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftHandFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftHandFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftHandFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftHandFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "abdomenFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "abdomenFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "abdomenFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "abdomenFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "abdomenFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "abdomenFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightHipFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightHipFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightHipFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightHipFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightHipFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightHipFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftHipFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftHipFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftHipFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftHipFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftHipFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftHipFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "groinFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "groinFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "groinFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "groinFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "groinFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "groinFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightThighFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightThighFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightThighFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightThighFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightThighFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightThighFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftThighFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftThighFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftThighFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftThighFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftThighFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftThighFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightKneeFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightKneeFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightKneeFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightKneeFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightKneeFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightKneeFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftKneeFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftKneeFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftKneeFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftKneeFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftKneeFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftKneeFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightLegFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightLegFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightLegFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightLegFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightLegFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightLegFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftLegFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftLegFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftLegFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftLegFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftLegFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftLegFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightAnkleFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightAnkleFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightAnkleFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightAnkleFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightAnkleFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightAnkleFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftAnkleFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftAnkleFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftAnkleFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftAnkleFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftAnkleFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftAnkleFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightFootFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightFootFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightFootFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightFootFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightFootFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightFootFront");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftFootFront.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftFootFront.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftFootFront.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftFootFront.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftFootFront.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftFootFront");

			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "headBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "headBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "headBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "headBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "headBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "headBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "neckBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "neckBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "neckBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "neckBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "neckBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "neckBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightShoulderBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightShoulderBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightShoulderBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightShoulderBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightShoulderBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightShoulderBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftShoulderBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftShoulderBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftShoulderBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftShoulderBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftShoulderBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftShoulderBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "thoracicSpineBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "thoracicSpineBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "thoracicSpineBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "thoracicSpineBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "thoracicSpineBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "thoracicSpineBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightArmBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightArmBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightArmBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightArmBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightArmBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightArmBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftArmBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftArmBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftArmBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftArmBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftArmBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftArmBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightElbowBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightElbowBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightElbowBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightElbowBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightElbowBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightElbowBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftElbowBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftElbowBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftElbowBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftElbowBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftElbowBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftElbowBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightForearmBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightForearmBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightForearmBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightForearmBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightForearmBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightForearmBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftForearmBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftForearmBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftForearmBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftForearmBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftForearmBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftForearmBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightWristBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightWristBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightWristBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightWristBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightWristBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightWristBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftWristBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftWristBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftWristBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftWristBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftWristBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftWristBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "lowerBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "lowerBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "lowerBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "lowerBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "lowerBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "lowerBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightButtockBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightButtockBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightButtockBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightButtockBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightButtockBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightButtockBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftButtockBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftButtockBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftButtockBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftButtockBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftButtockBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftButtockBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightThighBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightThighBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightThighBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightThighBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightThighBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightThighBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftThighBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftThighBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftThighBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftThighBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftThighBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftThighBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightKneeBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightKneeBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightKneeBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightKneeBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightKneeBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightKneeBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftKneeBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftKneeBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftKneeBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftKneeBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftKneeBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftKneeBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightLegBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightLegBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightLegBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightLegBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightLegBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightLegBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftLegBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftLegBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftLegBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftLegBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftLegBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftLegBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightAnkleBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightAnkleBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightAnkleBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightAnkleBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightAnkleBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightAnkleBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftAnkleBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftAnkleBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftAnkleBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftAnkleBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftAnkleBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftAnkleBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightFootBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightFootBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightFootBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightFootBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightFootBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightFootBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftFootBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftFootBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftFootBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftFootBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftFootBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftFootBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "rightHandBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "rightHandBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "rightHandBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "rightHandBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "rightHandBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "rightHandBack");
			options = new MyDictionary() { { 0, new ValuesOption() { Action = action, Text = "Burning", ActionId = "leftHandBack.Burning" } }, { 1, new ValuesOption() { Action = action, Text = "Numbness", ActionId = "leftHandBack.Numbness" } }, { 2, new ValuesOption() { Action = action, Text = "Pins-Needles", ActionId = "leftHandBack.PinsNeedles" } }, { 3, new ValuesOption() { Action = action, Text = "Stabbing", ActionId = "leftHandBack.Stabbing" } }, { 4, new ValuesOption() { Action = action, Text = "Ache", ActionId = "leftHandBack.Ache" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect, "leftHandBack");
		}
		#endregion

		public Program()
		{
			this.userClient = new UserClient();
			this.userClient.Login("admin", "Welc0me!");
			this.userClient.UserHasMultipleRoles("admin");
			this.userClient.SelectRole("admin", "Administrator");
			RequestHeader header = new RequestHeader();
			header.SessionId = this.userClient.GetConfiguration.SessionId;
			
			//string securityString = this.userClient.SecurityCookie;
			this.proClient = new ProClient();
			//this.proClient.SecurityCookie = securityString;
			this.proClient.RequestHeader = header;
			this.questionnaireClient = new QuestionnaireClient();
			this.questionnaireClient.RequestHeader = header;//.SecurityCookie = securityString;
			this.questionnaireFormatClient = new QuestionnaireFormatClient();
			this.questionnaireFormatClient.RequestHeader = header;//.SecurityCookie = securityString;
			this.userQuestionnaireClient = new PatientEpisodeClient();
			this.userQuestionnaireClient.RequestHeader = header;//.SecurityCookie = securityString;
			this.patientClient = new PatientClient();
			this.patientClient.RequestHeader = header;//.SecurityCookie = securityString;
			//TODO user has to login and security string set on all clients
		}

		public Survey CreateFirstAppointmentQuestionnaire()
		{
			Survey pro = new Survey();
			pro.Name = "OPSMCFirstAppointment";
			pro.DisplayName = "First Appointment";
			pro.Status = QuestionnaireStatus.Indevelopment;
			pro.IsActive = true;
			pro.DefaultFormatName = "OPSMCFirstAppointmentFormat";
			pro.Tags.Add(new Tag() { TagName = "Gender", Value = "Male" });
			pro.Tags.Add(new Tag() { TagName = "Gender", Value = "Female" });

			//TODO Define Domains

			pro.Concept = new QuestionnaireConcept() { Name = "FirstAppointment", Description = "OPSMC First Appointment Questionnaire" };

			{
				QuestionnaireSection section = new QuestionnaireSection() { OrderInInstrument = 1, Questionnaire = pro };
				pro.Sections.Add(section);
				AddTextToSection(section, "Intro.1", @"", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				AddTextToSection(section, "Intro.2", @"ACCOUNT SETTINGS", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);

				QuestionnaireItem qi = AddItemToSection(section, "OPSMC.1", ".", @"Hi there Laura! Welcome to RePLAY, a groundbreaking new service that allows you to track
								your health progress and prepare for your upcoming appointments at OPSMC. Here at OPSMC we take your privacy very seriously. We’ve put a lot
								of time and effort into keeping your information secure and you can see just how we do this by reading our privacy policy. Just click here to
								open the privacy policy in a new window. Next we’re going to choose a good password. "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				MyDictionary options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.2", Text = "Okay!  " } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

				qi = AddItemToSection(section, "OPSMC.2", ".", @"The best way to keep your information as secure as Fort Knox is to choose a strong password.
								This means your password will need to have <strong> at least: </strong> 8 characters, 
								a capital letter, a number and, a symbol. Please type in a password that fits these criteria, and make sure it’s one that you’ll remember! "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.4", Text = "Password" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "My password will be");

				qi = AddItemToSection(section, "OPSMC.3", ".", @"Hmmm… we think that password might be a little too easy to crack. Please enter a password that has
								<strong> at least: </strong> 8 characters, a capital letter, a number and, a symbol"
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.4", Text = "Stronger password" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Okay, ");

				qi = AddItemToSection(section, "OPSMC.4", ".", @"Perfect! To make sure we’ve got that down right, could you please type your super strong secret password again? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.6", Text = "Password again" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Sure, it is ");

				qi = AddItemToSection(section, "OPSMC.5", ".", @"Oops! Those passwords didn’t quite match. Please try re-typing your password.  "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "OPSMC.6", Text = "Same password as before" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "No problem ");

				qi = AddItemToSection(section, "OPSMC.6", ".", @"And just to be extremely sure, could you please type your password again?  "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.7", Text = "Confirm password" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Okay, ");

				qi = AddItemToSection(section, "OPSMC.7", ".", @"With this password your access to the system is secure enough. However, if you want to be extra cautious
								you could opt to have a second step in the login process whereby a security code will be sent to your mobile phone or email every time you
								want to login. What level of secure access would you like to have?   "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.13", Text = "User name and password " } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.8", Text = "User name, password and secure code via sms to your mobile phone " } }, { 2, new ValuesOption() { Action = "GOTO OPSMC.10", Text = "User name, password and secure code via your email  " } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

				qi = AddItemToSection(section, "OPSMC.8", ".", @"OK, can you confirm or correct your mobile number below?   "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.9", Text = "Mobile number" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Sure, my mobile number is ");

				qi = AddItemToSection(section, "OPSMC.9", ".", @"Thanks for that, a secure code has been sent to your mobile. Did you receive it?    "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.13", Text = "Yes, the security code is " } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.8", Text = "No, please send it again  " } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

				qi = AddItemToSection(section, "OPSMC.10", ".", @"Thanks for that, a secure code has been sent to your e-mail. Did you receive it?    "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.13", Text = "Yes, the security code is " } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.10", Text = "No, please send it again  " } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

				qi = AddItemToSection(section, "OPSMC.11", ".", @"Oops! That code is not correct. Please try re-typing your code.    "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.13", Text = "Your code" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Ok, the security code is ");

				qi = AddItemToSection(section, "OPSMC.12", ".", @"That is not working, please choose again what level of secure access you would like to have.
								What level of secure access would you like to have?   "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.13", Text = "User name and password " } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.8", Text = "User name, password and secure code via sms to your mobile phone " } }, { 2, new ValuesOption() { Action = "GOTO OPSMC.10", Text = "User name, password and secure code via your email  " } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

				qi = AddItemToSection(section, "OPSMC.13", ".", @"Thanks Laura! Now you can use your email address and password to log in to RePLAY whenever you like.
								Let’s get cracking on your First Visit questionnaire. Every patient is different so we’re going to ask you some general questions about
								your health and current condition to help us figure out the best possible way to treat you. It will take about 15 minutes to complete
								and you can do it on the train, at your local coffee shop, from the comfort of your own bed, or wherever else you may be! If you run
								out of time you can always save your answers and come back later.    "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.14", Text = "Sounds good!  " } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

				qi = AddItemToSection(section, "OPSMC.14", ".", @"We promise RePLAY is super easy to use, but if at any point you want to learn more about how it works you
								can click on the About RePLAY tab. Also, if you change your mind about any of your answers during the questionnaire, simply click the pencil
								icon to the right of your answer to edit it. Okay, are you ready to start?    "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.15", Text = "Yep, let’s start!  " } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

			}

			{
				QuestionnaireSection section = new QuestionnaireSection() { OrderInInstrument = 2, Questionnaire = pro };
				pro.Sections.Add(section);
				AddTextToSection(section, "Intro.3", @"", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				AddTextToSection(section, "Intro.4", @"PATIENT PERSONAL CONTACT DETAILS", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);

				QuestionnaireItem qi = AddItemToSection(section, "OPSMC.15", ".", @"Let’s start with the basics. We’ve filled in some of the information for you already.
								Could you please make sure we’ve got it right?"
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				MyDictionary options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.15.1", Text = "First Name" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "My name is ", "<%PatientFirstName%/>");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.15.1", Text = "Last Name" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, string.Empty, "<%PatientLastName%/>");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.15.1", Text = "" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List, ", and I was born on ", "<%PatientDateOfBirth%/>"); //Date picker

				qi = AddItemToSection(section, "OPSMC.15.1", ".", @"Please indicate your gender "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "Female" } }, { 1, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "Male" } }, { 2, new ValuesOption() { Action = "MakeItemApplicable", Text = "Other" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.ConditionalItem, "my gender is ", ActionId: "OPSMC.15.1.1");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.16", Text = "" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "");

				qi = AddItemToSection(section, "OPSMC.16", ".", @"Would you mind double-checking your e-mail address? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.17", Text = "e-mail" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Sure, my e-mail is ", "<%PatientEmail%/>");

				qi = AddItemToSection(section, "OPSMC.17", ".", @"What’s your home address? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.18", Text = "Country" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Country ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.18", Text = "State" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "State ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.18", Text = "Suburb" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Suburb ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.18", Text = "Unit number" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Unit number (if applicable)  ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.18", Text = "Street number" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Street number ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.18", Text = "Street name" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Street name ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.18", Text = "Postcode" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Postcode ");

				qi = AddItemToSection(section, "OPSMC.18", ".", @"And what’s your postal address?  "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "Same as my home address " } }, { 1, new ValuesOption() { Action = "MakeItemApplicable", Text = "This is my postal address" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.ConditionalItem);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.19", Text = "Country" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Country ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.19", Text = "State" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "State ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.19", Text = "Suburb" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Suburb ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.19", Text = "Unit number" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Unit number (if applicable)  ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.19", Text = "Street number" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Street number ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.19", Text = "Street name" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Street name ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.19", Text = "Postcode" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Postcode ");

				qi = AddItemToSection(section, "OPSMC.19", ".", @"What are the best phone numbers to contact you on?  "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.20", Text = "Home number" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "My home number is ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.20", Text = "Mobile number" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, ", my mobile number is ", "<%PatientPhoneNumber%/>");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.20", Text = "Business number" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "and my business number is ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.20", Text = "Home number" } }, { 1, new ValuesOption() { Action = "", Text = "Mobile number" } }, { 2, new ValuesOption() { Action = "", Text = "Business number" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List, "I prefer to be contacted on my ");

				qi = AddItemToSection(section, "OPSMC.20", ".", @"How do you prefer to be told about new questionnaires?  "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.21", Text = "By email " } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.21", Text = "By mobile " } }, { 2, new ValuesOption() { Action = "GOTO OPSMC.21", Text = "By email and mobile  " } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

				qi = AddItemToSection(section, "OPSMC.21", ".", @"In case of emergency, who can we contact and how?  "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.23", Text = "Name" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Name ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.23", Text = "Relationship" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Relationship ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.23", Text = "Phone" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Phone ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.23", Text = "Country" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Country ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.23", Text = "State" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "State ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.23", Text = "Suburb" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Suburb ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.23", Text = "Unit number" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Unit number (if applicable)  ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.23", Text = "Street number" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Street number ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.23", Text = "Street name" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Street name ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.23", Text = "Postcode" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Postcode ");

				qi = AddItemToSection(section, "OPSMC.22", ".", @"As you’re under 18 years of age, we need to grab your guardian’s contact details too. Could you please type them in below? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.23", Text = "Name" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Name ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.23", Text = "Relationship" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Relationship ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.23", Text = "Phone" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Phone ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.23", Text = "Country" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Country ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.23", Text = "State" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "State ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.23", Text = "Suburb" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Suburb ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.23", Text = "Unit number" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Unit number (if applicable)  ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.23", Text = "Street number" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Street number ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.23", Text = "Street name" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Street name ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.23", Text = "Postcode" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Postcode ");
			}

			{
				QuestionnaireSection section = new QuestionnaireSection() { OrderInInstrument = 3, Questionnaire = pro };
				pro.Sections.Add(section);
				AddTextToSection(section, "Intro.5", @"", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				AddTextToSection(section, "Intro.6", @"FAMILY, PAST AND MEDICAL HISTORY", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);

				QuestionnaireItem qi = AddItemToSection(section, "OPSMC.23", ".", @"Now let’s talk about your medical history. This information helps us to be sure
									that we’re treating you properly. First we have some questions about your family. Is there anyone in your family who has, or has had,
									any of the following? If so, please check the box. If none apply, just click ‘Skip’ to go to the next question. "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				MyDictionary options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.24", Text = "Diabetes", ActionId = "Diabetes" } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.24", Text = "Heart disease", ActionId = "Heart disease" } }, { 2, new ValuesOption() { Action = "GOTO OPSMC.24", Text = "Cancer", ActionId = "Cancer" } }, { 3, new ValuesOption() { Action = "GOTO OPSMC.24", Text = "Stroke", ActionId = "Stroke" } }, { 4, new ValuesOption() { Action = "GOTO OPSMC.24", Text = "High blood pressure", ActionId = "High blood pressure" } }, { 5, new ValuesOption() { Action = "GOTO OPSMC.24", Text = "Arthritis", ActionId = "Arthritis" } }, { 6, new ValuesOption() { Action = "GOTO OPSMC.24", Text = "Other", ActionId = "Other" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect);

				qi = AddItemToSection(section, "OPSMC.24", ".", @"(Constitutional) </br> Do you currently have, or have you ever had, any problems related to any of the following?
									Please tick all that apply. If none apply, just click ‘Skip’ to go to the next question. "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.25", Text = "Unexplained Fever", ActionId = "Unexplained Fever" } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.25", Text = "Chills", ActionId = "Chills" } }, { 2, new ValuesOption() { Action = "GOTO OPSMC.25", Text = "Night sweats", ActionId = "Night sweats" } }, { 3, new ValuesOption() { Action = "GOTO OPSMC.25", Text = "Unexplained weight loss", ActionId = "Unexplained weight loss" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect);

				qi = AddItemToSection(section, "OPSMC.25", ".", @"(Eyes, ENT and Physiological) </br> Do you currently have, or have you ever had, any problems related to any of the following?
									Please tick all that apply. If none apply, just click ‘Send’ to skip to the next question. "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.26", Text = "Visual changes", ActionId = "Visual changes" } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.26", Text = "Glaucoma", ActionId = "Glaucoma" } }, { 2, new ValuesOption() { Action = "GOTO OPSMC.26", Text = "Loss of hearing", ActionId = "Loss of hearing" } }, { 3, new ValuesOption() { Action = "GOTO OPSMC.26", Text = "Sinus problems", ActionId = "Sinus problems" } }, { 4, new ValuesOption() { Action = "GOTO OPSMC.26", Text = "Depression", ActionId = "Depression" } }, { 5, new ValuesOption() { Action = "GOTO OPSMC.26", Text = "Sleeping disorders", ActionId = "Sleeping disorders" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect);

				qi = AddItemToSection(section, "OPSMC.26", ".", @"(Respiratory) </br> Do you currently have, or have you ever had, any problems related to any of the following?
									Please tick all that apply. If none apply, just click ‘Send’ to skip to the next question. "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Pneumonia", ActionId = "Pneumonia" } }, { 1, new ValuesOption() { Action = "", Text = "Asthma ", ActionId = "Asthma " } }, { 2, new ValuesOption() { Action = "", Text = "Loss of hearing", ActionId = "Loss of hearing" } }, { 3, new ValuesOption() { Action = "", Text = "Emphysema", ActionId = "Emphysema" } }, { 4, new ValuesOption() { Action = "", Text = "Bronchitis", ActionId = "Bronchitis" } }, { 5, new ValuesOption() { Action = "", Text = "Tuberculosis", ActionId = "Tuberculosis" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect);

				qi = AddItemToSection(section, "OPSMC.27", ".", @"(Cardiovascular) </br> Do you currently have, or have you ever had, any problems related to any of the following?
									Please tick all that apply. If none apply, just click ‘Send’ to skip to the next question. "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Heart attack", ActionId = "Heart attack" } }, { 1, new ValuesOption() { Action = "", Text = "Mitral Valve Prolapse", ActionId = "Mitral Valve Prolapse" } }, { 2, new ValuesOption() { Action = "", Text = "Abdominal heart Rhythm", ActionId = "Abdominal heart Rhythm" } }, { 3, new ValuesOption() { Action = "", Text = "High blood pressure", ActionId = "High blood pressure" } }, { 4, new ValuesOption() { Action = "", Text = "Stroke/ Mini-stroke", ActionId = "StrokeMiniStroke" } }, { 5, new ValuesOption() { Action = "", Text = "Aneurysm", ActionId = "Aneurysm" } }, { 6, new ValuesOption() { Action = "", Text = "Poor circulation in legs", ActionId = "Poor circulation in legs" } }, { 7, new ValuesOption() { Action = "", Text = "Raynaud’s disease", ActionId = "Raynaud’s disease" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect);

				qi = AddItemToSection(section, "OPSMC.28", ".", @"(Musculoskeletal) </br> Do you currently have, or have you ever had, any problems related to any of the following?
									Please tick all that apply. If none apply, just click ‘Send’ to skip to the next question. "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Back pain", ActionId = "Back pain" } }, { 1, new ValuesOption() { Action = "", Text = "Osteoporosis", ActionId = "Osteoporosis" } }, { 2, new ValuesOption() { Action = "", Text = "Arthritis", ActionId = "Arthritis" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect);

				qi = AddItemToSection(section, "OPSMC.29", ".", @"(Gastrointestinal) </br> Do you currently have, or have you ever had, any problems related to any of the following?
									Please tick all that apply. If none apply, just click ‘Send’ to skip to the next question.  "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Abdominal pain", ActionId = "Abdominal pain" } }, { 1, new ValuesOption() { Action = "", Text = "Blood in stool", ActionId = "Blood in stool" } }, { 2, new ValuesOption() { Action = "", Text = "Nauseas/vomiting (Not due to flu)" } }, { 3, new ValuesOption() { Action = "", Text = "Indigestion/heartburn", ActionId = "Indigestion/heartburn" } }, { 4, new ValuesOption() { Action = "", Text = "Stomach ulcers", ActionId = "Stomach ulcers" } }, { 5, new ValuesOption() { Action = "", Text = "Irritable Bowel Syndrome", ActionId = "Irritable Bowel Syndrome" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect);

				qi = AddItemToSection(section, "OPSMC.30", ".", @"(Miscellaneous) </br> Do you currently have, or have you ever had, any problems related to any of the following?
									Please tick all that apply. If none apply, just click ‘Send’ to skip to the next question. "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "History of alcohol abuse", ActionId = "History of alcohol abuse" } }, { 1, new ValuesOption() { Action = "", Text = "History of drug abuse", ActionId = "History of drug abuse" } }, { 2, new ValuesOption() { Action = "", Text = "Hepatitis", ActionId = "Hepatitis" } }, { 3, new ValuesOption() { Action = "", Text = "Liver disorders ", ActionId = "Liver disorders " } }, { 4, new ValuesOption() { Action = "", Text = "Indigestion/heartburn", ActionId = "Indigestion/heartburn" } }, { 5, new ValuesOption() { Action = "", Text = "Stomach ulcers", ActionId = "Stomach ulcers" } }, { 6, new ValuesOption() { Action = "", Text = "Irritable Bowel Syndrome", ActionId = "Irritable Bowel Syndrome" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect);

				qi = AddItemToSection(section, "OPSMC.31", ".", @"(Integumentary/breast) </br> Do you currently have, or have you ever had, any problems related to any of the following?
									Please tick all that apply. If none apply, just click ‘Send’ to skip to the next question. "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Skin rash", ActionId = "Skin rash" } }, { 1, new ValuesOption() { Action = "", Text = "Sore that will not heal", ActionId = "Sore that will not heal" } }, { 2, new ValuesOption() { Action = "", Text = "Breast lump/discharge", ActionId = "Breast lump/discharge" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect);

				qi = AddItemToSection(section, "OPSMC.32", ".", @"(Hematologic/Lymphatic) </br> Do you currently have, or have you ever had, any problems related to any of the following?
									Please tick all that apply. If none apply, just click ‘Send’ to skip to the next question. "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Anaemia", ActionId = "Anaemia" } }, { 1, new ValuesOption() { Action = "", Text = "Bruise easily", ActionId = "Bruise easily" } }, { 2, new ValuesOption() { Action = "", Text = "Sickle Cell Disorders", ActionId = "Sickle Cell Disorders" } }, { 3, new ValuesOption() { Action = "", Text = "Blood clots", ActionId = "Blood clots" } }, { 4, new ValuesOption() { Action = "", Text = "Aids or HIV", ActionId = "Aids or HIV" } }, { 5, new ValuesOption() { Action = "", Text = "Enlarged Lymph Nodes", ActionId = "Enlarged Lymph Nodes" } }, { 6, new ValuesOption() { Action = "", Text = "Lupus", ActionId = "Lupus" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect);

				qi = AddItemToSection(section, "OPSMC.33", ".", @"(Genitourinary) </br> Do you currently have, or have you ever had, any problems related to any of the following?
									Please tick all that apply. If none apply, just click ‘Send’ to skip to the next question. "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Bladder problems", ActionId = "Bladder problems" } }, { 1, new ValuesOption() { Action = "", Text = "Frequent urinary infection(s)" } }, { 2, new ValuesOption() { Action = "", Text = "Blood in urine", ActionId = "Blood in urine" } }, { 3, new ValuesOption() { Action = "", Text = "Blood clots", ActionId = "Blood clots" } }, { 4, new ValuesOption() { Action = "", Text = "Kidney stones", ActionId = "Kidney stones" } }, { 5, new ValuesOption() { Action = "", Text = "Kidney failure", ActionId = "Kidney failure" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect);

				qi = AddItemToSection(section, "OPSMC.34", ".", @"(Neurological) </br> Do you currently have, or have you ever had, any problems related to any of the following?
									Please tick all that apply. If none apply, just click ‘Send’ to skip to the next question. "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Dizzy spells", ActionId = "Dizzy spells" } }, { 1, new ValuesOption() { Action = "", Text = "Seizures or Convulsions", ActionId = "Seizures or Convulsions" } }, { 2, new ValuesOption() { Action = "", Text = "Headaches", ActionId = "Headaches" } }, { 3, new ValuesOption() { Action = "", Text = "Multiple Sclerosis", ActionId = "Multiple Sclerosis" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect);

				qi = AddItemToSection(section, "OPSMC.35", ".", @"(Endocrines) </br> Do you currently have, or have you ever had, any problems related to any of the following?
									Please tick all that apply. If none apply, just click ‘Send’ to skip to the next question. "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.36", Text = "Diabetes", ActionId = "Diabetes" } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.36", Text = "Thyroid dysfunction", ActionId = "Thyroid dysfunction" } }, { 2, new ValuesOption() { Action = "GOTO OPSMC.36", Text = "Gout", ActionId = "Gout" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect);

				qi = AddItemToSection(section, "OPSMC.36", ".", @"(Cancer) </br> Do you currently have, or have you ever had, cancer?"
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "No" } }, { 1, new ValuesOption() { Action = "MakeItemApplicable", Text = "Yes" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Type of cancer" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Please specify what type ");

				//                qi = AddItemToSection(section, "OPSMC.37", ".", @"Have you ever had an operation? If so, please tell us what type and the year that you had it. Just click the ‘+’
				//                                icon to add more operations. "
				//                                , Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				//                options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "No" } }, { 1, new ValuesOption() { Action = "MakeItemApplicable", Text = "Yes" } } };
				//                AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.ConditionalItem);
				//                options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Type of operation" } } };
				//                AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "I had: ");
				//                options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "" } } };
				//                AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List, "Year: "); //Date picker

				qi = AddItemToSection(section, "OPSMC.37", ".", @"Have you ever had an operation? If so, please tell us what type and the year that you had it. "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "No" } }, { 1, new ValuesOption() { Action = "MakeItemApplicable", Text = "Yes" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.ConditionalItem);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Type of operation(s) and year(s)" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "I had ");

				//                qi = AddItemToSection(section, "OPSMC.38", ".", @"Do you have any allergies at all? If so, please tell us what you’re allergic to. Just click the ‘+’ icon to add
				//                                more allergies. "
				//                                , Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				//                options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "No" } }, { 1, new ValuesOption() { Action = "MakeItemApplicable", Text = "Yes" } } };
				//                AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);
				//                options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Allergie" } } };
				//                AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "I'm allergic to ");

				qi = AddItemToSection(section, "OPSMC.38", ".", @"Do you have any allergies at all? If so, please tell us what you’re allergic to. "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "No" } }, { 1, new ValuesOption() { Action = "MakeItemApplicable", Text = "Yes" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Allergie(s)" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "I'm allergic to ");

				qi = AddItemToSection(section, "OPSMC.39", ".", @"How much do you weigh and how tall are you? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Weight", ActionId = "OPSMC.39.1.1" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "I weigh", ActionId: "OPSMC.39.1");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Height", ActionId = "OPSMC.39.2" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "kg, and my height is ", ActionId: "OPSMC.39.2");

				qi = AddItemToSection(section, "OPSMC.40", ".", @"Is there any possibility that you might be pregnant? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.41", Text = "No" } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.41", Text = "Yes" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);
			}

			{
				QuestionnaireSection section = new QuestionnaireSection() { OrderInInstrument = 4, Questionnaire = pro };
				pro.Sections.Add(section);
				AddTextToSection(section, "Intro.7", @"", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				AddTextToSection(section, "Intro.8", @"SOCIAL AND SPORTING HISTORY", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);

				QuestionnaireItem qi = AddItemToSection(section, "OPSMC.41", ".", @"All right, now onto your social and sporting history. <br/>
								What is your occupation? Start typing then please select the option that most accurately describes what you do. "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				MyDictionary options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.42", Text = "Occupation" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "I am a ");

				qi = AddItemToSection(section, "OPSMC.42", ".", @"How would you describe your type of work? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.43", Text = "not doing right now" } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.43", Text = "working at DS-PRIMA" } }, { 2, new ValuesOption() { Action = "GOTO OPSMC.43", Text = "working at a hospital" } }, { 3, new ValuesOption() { Action = "GOTO OPSMC.43", Text = "still deciding between DS-PRIMA and the hospital" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List, "My work could be considered");

				qi = AddItemToSection(section, "OPSMC.43", ".", @"What is your current work status? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.44", Text = "Full time" } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.44", Text = "Part time" } }, { 2, new ValuesOption() { Action = "GOTO OPSMC.44", Text = "Current" } }, { 3, new ValuesOption() { Action = "GOTO OPSMC.44", Text = "Unemployed at the moment" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List, "My position is ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.44", Text = "Unrestricted" } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.44", Text = "Restricted duties" } }, { 2, new ValuesOption() { Action = "GOTO OPSMC.44", Text = "Alternative duties" } }, { 3, new ValuesOption() { Action = "GOTO OPSMC.44", Text = "I’m unfit for duties" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List, "and the limitations of my duties are ");

				qi = AddItemToSection(section, "OPSMC.44", ".", @"Do you currently practice, or have you ever practiced, any physical activity or sport? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.48", Text = "No" } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.45", Text = "Yes" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

				qi = AddItemToSection(section, "OPSMC.45", ".", @"What kind of physical activities or sports do you currently practice? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.46", Text = "Soccer" } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.46", Text = "Baseball" } }, { 2, new ValuesOption() { Action = "GOTO OPSMC.46", Text = "Swimming" } }, { 3, new ValuesOption() { Action = "GOTO OPSMC.46", Text = "TV watching" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List, "My main activity is ", ActionId: "OPSMC.45.1");
                AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List, "My secondary activity is ", ActionId: "OPSMC.45.2");
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List, "I also do ");

				qi = AddItemToSection(section, "OPSMC.46", ".", @"What’s your current sporting level and what’s the highest level you’ve ever reached? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.47", Text = "International" } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.47", Text = "National" } }, { 2, new ValuesOption() { Action = "GOTO OPSMC.47", Text = "State" } }, { 3, new ValuesOption() { Action = "GOTO OPSMC.47", Text = "Club" } }, { 4, new ValuesOption() { Action = "GOTO OPSMC.47", Text = "Recreational" } }, { 5, new ValuesOption() { Action = "GOTO OPSMC.47", Text = "Past participant" } }, { 6, new ValuesOption() { Action = "GOTO OPSMC.47", Text = "Nil" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List, "My current level is ", ActionId: "OPSMC.46.1");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.47", Text = "International" } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.47", Text = "National" } }, { 2, new ValuesOption() { Action = "GOTO OPSMC.47", Text = "State" } }, { 3, new ValuesOption() { Action = "GOTO OPSMC.47", Text = "Club" } }, { 4, new ValuesOption() { Action = "GOTO OPSMC.47", Text = "Recreational" } }, { 5, new ValuesOption() { Action = "GOTO OPSMC.47", Text = "Nil" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List, "My highest level was ");

				qi = AddItemToSection(section, "OPSMC.47", ".", @"In the past week, how many training sessions have you done? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.48", Text = "Number of traning sessions" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "I trained");

				qi = AddItemToSection(section, "OPSMC.48", ".", @"Which is your dominant side? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.49", Text = "I am right handed" } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.49", Text = "I am left handed" } }, { 2, new ValuesOption() { Action = "GOTO OPSMC.49", Text = "I am ambidextrous" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

				qi = AddItemToSection(section, "OPSMC.49", ".", @"What’s your ethnic background? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.50", Text = "from here" } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.50", Text = "from there" } }, { 2, new ValuesOption() { Action = "GOTO OPSMC.50", Text = "from over there" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List, "I am");

				qi = AddItemToSection(section, "OPSMC.50", ".", @"Do you smoke? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "No, I never have" } }, { 1, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "I did, but I quit" } }, { 2, new ValuesOption() { Action = "MakeItemApplicable", Text = "Yes, I smoke" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.ConditionalItem);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Number of cigarretes" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "cigarettes per day ");

				qi = AddItemToSection(section, "OPSMC.51", ".", @"How often do you drink alcohol? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "Never" } }, { 1, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "Only on special occasions" } }, { 2, new ValuesOption() { Action = "MakeItemApplicable", Text = "Regularly" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.ConditionalItem);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.52", Text = "Number of drinks" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "I have approximately");

				//qi = AddItemToSection(section, "OPSMC.52", ".", @"Do you take any supplements? Just click the ‘+’ icon to add more supplements. "
				//                , Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				//options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "No" } }, { 1, new ValuesOption() { Action = "MakeItemApplicable", Text = "Yes" } } };
				//AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.ConditionalItem);
				//options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Indicate supplement" } } };
				//AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "I take");

				qi = AddItemToSection(section, "OPSMC.52", ".", @"Do you take any supplements? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "No" } }, { 1, new ValuesOption() { Action = "MakeItemApplicable", Text = "Yes" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.ConditionalItem);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Indicate supplement(s)" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "I take");

				qi = AddItemToSection(section, "OPSMC.53", ".", @"Are you on a special diet? If so, could you please describe what your diet involves? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "No" } }, { 1, new ValuesOption() { Action = "MakeItemApplicable", Text = "Yes" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.ConditionalItem);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.54", Text = "Indicate your diet" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "my diet is");
			}

			{
				QuestionnaireSection section = new QuestionnaireSection() { OrderInInstrument = 5, Questionnaire = pro };
				pro.Sections.Add(section);
				AddTextToSection(section, "Intro.9", @"", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				AddTextToSection(section, "Intro.10", @"PRESENT COMPLAINT", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);

				QuestionnaireItem qi = AddItemToSection(section, "OPSMC.54", ".", @"We’d like to know more about the issue that brought you to OPSMC in the first place. 
								On the body diagram below, please tick the part(s) of the body that are affected by your current condition. Then tell us what kinds of sensations you’re 
								experiencing as a result of your condition. Select as many areas and sensations as needed. "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				addBodyControl(section, qi, "GOTO OPSMC.55");

				qi = AddItemToSection(section, "OPSMC.55", ".", @"How would you rate your average pain level? Click on the following scale, 0% means no pain and 100% means the worst
								possible pain. "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				MyDictionary options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.56", Text = "No pain" } }, { 100, new ValuesOption() { Action = "GOTO OPSMC.55", Text = "Worst possible pain" } } };
				AddOptionGroupToItem(qi, options, 10, QuestionnaireResponseType.Range);

				//                qi = AddItemToSection(section, "OPSMC.56", ".", @"Have you had the same or a similar condition before? If so, please tell us what you had and when.
				//                                Just click the ‘+’ icon to add more conditions. "
				//                                , Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				//                options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "No, I haven't" } }, { 1, new ValuesOption() { Action = "MakeItemApplicable", Text = "Yes" } } };
				//                AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.ConditionalItem);
				//                options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.57", Text = "Add conditions" } } };
				//                AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "I had: ");

				qi = AddItemToSection(section, "OPSMC.56", ".", @"Have you had the same or a similar condition before? If so, please tell us what you had and when. "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "No, I haven't" } }, { 1, new ValuesOption() { Action = "MakeItemApplicable", Text = "Yes" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.ConditionalItem);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.57", Text = "Add conditions" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "I had ");

				qi = AddItemToSection(section, "OPSMC.57", ".", @"Before we move on, is there anything else we should know that might help us take better care of you? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "No, that’s all" } }, { 1, new ValuesOption() { Action = "MakeItemApplicable", Text = "Yes" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.ConditionalItem);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.58", Text = "Indicate aditional relevant information" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text);
			}

			{
				QuestionnaireSection section = new QuestionnaireSection() { OrderInInstrument = 6, Questionnaire = pro };
				pro.Sections.Add(section);
				AddTextToSection(section, "Intro.11", @"", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				AddTextToSection(section, "Intro.12", @"PRESENT COMPLAINT", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);

				QuestionnaireItem qi = AddItemToSection(section, "OPSMC.58", ".", @"We’re almost done! But before we finish we need to ask you for some basic administrative
								information. Do you have Medicare? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				MyDictionary options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "No" } }, { 1, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "Yes, but I don’t know my Medicare number" } }, { 2, new ValuesOption() { Action = "MakeItemApplicable", Text = "Yes" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.ConditionalItem);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.59", Text = "Medicare number" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, ",my medicare number is");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.59", Text = "Ref Number" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Ref Number ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.59", Text = "" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List, "Expiry date "); //Date picker

				qi = AddItemToSection(section, "OPSMC.59", ".", @"Do you have health insurance? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "No" } }, { 1, new ValuesOption() { Action = "MakeItemApplicable", Text = "Yes" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.ConditionalItem);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.60", Text = "Insurance company" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, ",my health insurance company is ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.60", Text = "Policy Number" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Policy Number ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.60", Text = "Member Number" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Member number ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.60", Text = "Extra cover" } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.60", Text = "No extra cover" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List, "Cover ");

				qi = AddItemToSection(section, "OPSMC.60", ".", @"Is your appointment workcover related? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.62", Text = "No" } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.61", Text = "Yes" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

				qi = AddItemToSection(section, "OPSMC.61", ".", @"Could you please give us your employer’s details? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.62", Text = "Employer" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Employer ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.62", Text = "Contact number" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Contact number ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.62", Text = "Address" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Address ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.62", Text = "" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List, "Accident date "); //Date picker
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.62", Text = "Insurance agent" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Insurance agent ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.62", Text = "Claim number" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Claim number ");

				qi = AddItemToSection(section, "OPSMC.62", ".", @"Is your appointment related to a Transport Accident Commission (TAC) injury claim? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.64", Text = "No" } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.63", Text = "Yes" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

				qi = AddItemToSection(section, "OPSMC.63", ".", @"Could you please give us your employer’s details? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.64", Text = "" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List, "Date of accident "); //Date picker
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.64", Text = "Claim number" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Claim number ");

				qi = AddItemToSection(section, "OPSMC.64", ".", @"Are you a veteran? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.66", Text = "No" } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.65", Text = "Yes" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

				qi = AddItemToSection(section, "OPSMC.65", ".", @"What are your Veterans Affairs details? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Veterans Affairs number" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Veterans Affairs number ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Expiry date" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List, "Expiry date ");  //Date picker

				qi = AddItemToSection(section, "OPSMC.66", ".", @"How were you referred to OPSMC? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.68", Text = "Self- via website/social media,etc." } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.68", Text = "Friend/family" } }, { 2, new ValuesOption() { Action = "GOTO OPSMC.68", Text = "Employer" } }, { 3, new ValuesOption() { Action = "GOTO OPSMC.68", Text = "Coach/Club" } }, { 4, new ValuesOption() { Action = "GOTO OPSMC.67", Text = "External practitioner" } }, { 5, new ValuesOption() { Action = "GOTO OPSMC.68", Text = "Oral or written referral" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect);

				qi = AddItemToSection(section, "OPSMC.67", ".", @"Could you please pass on a few details about the practitioner that referred you? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.68", Text = "Name" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Name ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.68", Text = "Contact number" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Contact number ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.68", Text = "Address" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "Address ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.68", Text = "Doctor" } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.68", Text = "Physio" } }, { 2, new ValuesOption() { Action = "GOTO OPSMC.68", Text = "Masseur" } }, { 3, new ValuesOption() { Action = "GOTO OPSMC.68", Text = "Podiatrist" } }, { 4, new ValuesOption() { Action = "GOTO OPSMC.68", Text = "Other" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List, "Specialisation ");

				qi = AddItemToSection(section, "OPSMC.68", ".", @"How did you hear about OPSMC? "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.69", Text = "Professional referral" } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.69", Text = "Signs" } }, { 2, new ValuesOption() { Action = "GOTO OPSMC.69", Text = "Yellow pages" } }, { 3, new ValuesOption() { Action = "", Text = "Twitter" } }, { 4, new ValuesOption() { Action = "", Text = "Family/Friend" } }, { 5, new ValuesOption() { Action = "", Text = "Media" } }, { 6, new ValuesOption() { Action = "", Text = "White pages" } }, { 7, new ValuesOption() { Action = "", Text = "Facebook" } }, { 8, new ValuesOption() { Action = "", Text = "Google" } }, { 9, new ValuesOption() { Action = "", Text = "Other" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.ConditionalItem);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.69", Text = "" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text);
			}

			{
				QuestionnaireSection section = new QuestionnaireSection() { OrderInInstrument = 7, Questionnaire = pro };
				pro.Sections.Add(section);
				AddTextToSection(section, "Intro.13", @"", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				AddTextToSection(section, "Intro.14", @"PRIVACY SETTINGS", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);

				QuestionnaireItem qi = AddItemToSection(section, "OPSMC.69", ".", @"Here at OPSMC we take your privacy and confidentiality very seriously and make
								every effort to keep your information secure. Please tell us how would you like us to share your information within OPSMC. Whichever
								option you select, we will always abide by our strict privacy and confidentiality policy.  "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				MyDictionary options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.70", Text = "Openly" } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.70", Text = "Confidentially" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List, "For OPSMC’s quality improvement ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.70", Text = "Openly" } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.70", Text = "Confidentially" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List, "For OPSMC’s administrative purposes ");
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.70", Text = "Openly" } }, { 1, new ValuesOption() { Action = "GOTO OPSMC.70", Text = "Confidentially" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List, "For research purposes ");

				qi = AddItemToSection(section, "OPSMC.70", ".", @"Here at OPSMC we take your privacy and confidentiality very seriously and make
								every effort to keep your information secure. Please tell us how would you like us to share your information within OPSMC. Whichever
								option you select, we will always abide by our strict privacy and confidentiality policy.  "
								, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "I have read and accept OPSMC’s payment conditions" } }, { 1, new ValuesOption() { Action = "", Text = "I have read and accept OPSMC’S Privacy and confidentiality policy" } }, { 2, new ValuesOption() { Action = "", Text = "I have read and accept the RePLAY terms and conditions" } } };
				AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List, "For OPSMC’s quality improvement ");
			}

			{
				QuestionnaireSection section = new QuestionnaireSection() { OrderInInstrument = 8, Questionnaire = pro };
				pro.Sections.Add(section);
				AddTextToSection(section, "Footer.1", @"And that’s that! Thank you for taking the time to answer those questions. The information you just
								provided will enable us to really tailor your treatment plan to your condition and needs. If you need to edit any of your
								answers, please do so now. Otherwise, just click ‘submit’ to confirm your answers and you’re good to go! If you have any urgent
								issues or questions, please call OPSMC on 1300 859 887 or 9420 4300 to talk directly to your practitioner. Thanks again and we 
								look forward to continuing your care at Olympic Park Sports Medicine Centre. ", Platform.Classic, Instance.Baseline | Instance.Followup);
			}

			return pro;
		}
		public ProInstrument CreateInstrumentNEW()
		{
			// *** Configuration ***
			string path = "C:\\Users\\August\\Documents\\MIXML.xml";

			// *** Process ***
			ProInstrument proinstrument = new ProInstrument();
			XmlDocument reader = new XmlDocument();
			reader.Load(path);

			// Falta añadir los tags
			proinstrument.Tags.Add(new Tag() { TagName = "Gender", Value = "Male" });
			proinstrument.Tags.Add(new Tag() { TagName = "Gender", Value = "Female" });
			// -------------------------------------------------------------

			#region Questionnaire
			XmlNodeList listaNodos = reader.SelectNodes("data/Questionnaire");
			XmlNode Questionnaire;

			for (int i = 0; i < listaNodos.Count; i++)
			{
				Questionnaire = listaNodos.Item(i);

				proinstrument.Name = Questionnaire.SelectSingleNode("Name").InnerText;
				string ExtendedName = Questionnaire.SelectSingleNode("ExtendedName").InnerText;
				string DateFormat = Questionnaire.SelectSingleNode("DateFormat").InnerText; // Don't know where to send this
				if (Questionnaire.SelectSingleNode("Active").InnerText == "true") { proinstrument.IsActive = true; } else { proinstrument.IsActive = false; }
				if (Questionnaire.SelectSingleNode("Status").InnerText == "InDevelopment") { proinstrument.Status = QuestionnaireStatus.Indevelopment; }
				proinstrument.Concept = new QuestionnaireConcept() { Name = proinstrument.Name, Description = ExtendedName };
			}
			#endregion
			#region Header
			/*listaNodos = reader.SelectNodes("data/Header");
				XmlNode Header;

				for (int i = 0; i < listaNodos.Count; i++)
				{
					Header = listaNodos.Item(i);

					string InitialQuestion = Header.SelectSingleNode("InitialQuestion").InnerText;
					//Label1.Text = Label1.Text + InitialQuestion + "<br/>";
					XmlNodeList InitialQuestionAnswers = Header.SelectSingleNode("InitialQuestionAnswers").SelectNodes("Answer");
					XmlNode Answer;

					for (int f = 0; f < InitialQuestionAnswers.Count; f++)
					{
						Answer = InitialQuestionAnswers.Item(f);

						string CurrentAnswer = Answer.InnerText;
						//Label1.Text =Label1.Text + CurrentAnswer + "<br/>";
					}

				}*/
			#endregion
			#region QuestionsHeader
			QuestionnaireSection section = new QuestionnaireSection() { OrderInInstrument = 1, Questionnaire = proinstrument };
			proinstrument.Sections.Add(section);

			XmlNodeList listaQuestionsHeader = reader.SelectNodes("data/QuestionsHeader");
			XmlNode QuestionsHeader;

			for (int i = 0; i < listaQuestionsHeader.Count; i++)
			{
				QuestionsHeader = listaQuestionsHeader.Item(i);

				string QuestionTitle = QuestionsHeader.SelectSingleNode("QuestionTitle").InnerText;
				string Note = QuestionsHeader.SelectSingleNode("Note").InnerText;
				string Instruction = QuestionsHeader.SelectSingleNode("Instruction").InnerText;
				string PerQuestionInstruction = QuestionsHeader.SelectSingleNode("PerQuestionInstruction").InnerText;

				AddTextToSection(section, "QuestionTitle", QuestionTitle, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				AddTextToSection(section, "Instruction", Instruction, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				AddTextToSection(section, "PerQuestionInstruction", PerQuestionInstruction, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				AddTextToSection(section, "Note", Note, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);

			}
			#endregion
			#region Questions
			QuestionnaireSection ssection = new QuestionnaireSection() { OrderInInstrument = 2, Questionnaire = proinstrument };
			proinstrument.Sections.Add(ssection);

			listaNodos = reader.SelectNodes("data/Questions");
			XmlNode QQuestion;

			// Label1.Text = listaNodos.Count.ToString();
			for (int i = 0; i < listaNodos.Count; i++)
			{
				QQuestion = listaNodos.Item(i);

				XmlNodeList Questions = QQuestion.SelectNodes("Question");

				XmlNode Question;
				XmlNode Answer;

				for (int f = 0; f < Questions.Count; f++)
				{
					Question = Questions.Item(f);

					string CurrentQuestion = Question.SelectSingleNode("Text").InnerText;
					//Label1.Text = Label1.Text + CurrentQuestion + "<br/>";

					QuestionnaireItem QuestionItem = AddItemToSection(ssection, proinstrument.Name + "." + f.ToString(), f.ToString() + ".", CurrentQuestion, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);

					XmlNodeList CurrentQuestionAnswers = Question.SelectSingleNode("Answers").SelectNodes("Answer");
					MyDictionary CurrentQQuestionAnswers = new MyDictionary();

					for (int g = CurrentQuestionAnswers.Count - 1; g >= 0; g--)
					{
						Answer = CurrentQuestionAnswers.Item(g);
						string CurrentAnswer = Answer.InnerText;
						//Label1.Text = Label1.Text + CurrentAnswer + "<br/>";

						CurrentQQuestionAnswers.Add(g, new ValuesOption() { Action = "", Text = CurrentAnswer });

						AddOptionGroupToItem(QuestionItem, CurrentQQuestionAnswers, 0, QuestionnaireResponseType.List);

					}


				}

			}
			#endregion
			#region Scores

			ProDomain SScore = new ProDomain();
			SScore.Name = "Scores";
			SScore.Audience = UserTypes.Patient;
			SScore.Description = " ";

			listaNodos = reader.SelectNodes("data/Scores");
			XmlNode Scores;

			for (int i = 0; i < listaNodos.Count; i++)
			{
				Scores = listaNodos.Item(i);

				string MetricScore = Scores.SelectSingleNode("MetricScore").InnerText;
				SScore.ScoreFormula = MetricScore; // Listo

				XmlNodeList SScores = Scores.SelectNodes("Score");
				XmlNode Score;

				for (int f = 0; f < SScores.Count; f++)
				{
					ProDomainResultRange SScoreResultRange = new ProDomainResultRange();
					Score = SScores.Item(f);

					string Min = Score.SelectSingleNode("Min").InnerText;
					string Max = Score.SelectSingleNode("Max").InnerText;
					string Message = Score.SelectSingleNode("Message").InnerText;

					SScoreResultRange.Start = int.Parse(Min);
					SScoreResultRange.End = int.Parse(Max);
					SScoreResultRange.Meaning = Message;

					SScore.ResultRanges.Add(SScoreResultRange); // Listo
				}

			}
			proinstrument.Domains.Add(SScore);
			#endregion
			#region Domains
			// ESTO LO CALCULA SOLO EL PROGRAMA ****
			/*
			listaNodos = reader.SelectNodes("data/Domains");
			XmlNode Domain;

			for (int i = 0; i < listaNodos.Count; i++)
			{
				Domain = listaNodos.Item(i);

				XmlNodeList DomainName = Domain.SelectSingleNode("Domain").SelectNodes("Name");
				XmlNode DName;
				for (int f = 0; f < DomainName.Count; f++)
				{
					DName = DomainName.Item(f);
					string CurrentDomain = DName.InnerText;

					//ProDDomain.Name = CurrentDomain; // Domain Name
				}

				XmlNodeList DQuestion = Domain.SelectSingleNode("Domain").SelectNodes("Question");
				XmlNode DomainQuestion;

				for (int f = 0; f < DQuestion.Count; f++)
				{
					DomainQuestion = DQuestion.Item(f);

				  string CurrentDomainQuestion = DomainQuestion.InnerText;
				  //Label1.Text = Label1.Text + CurrentDomainQuestion + "<br/>";
				}

				XmlNodeList MaximumScoreDomain = Domain.SelectSingleNode("Domain").SelectNodes("MaximumScoreDomain");
				XmlNode MaxScoreDomain;

				for (int f = 0; f < MaximumScoreDomain.Count; f++)
				{
					MaxScoreDomain = MaximumScoreDomain.Item(f);

				   string CurrentMaxScoreDomain = MaxScoreDomain.InnerText;
				   //Label1.Text = Label1.Text + CurrentMaxScoreDomain + "<br/>";

				}
			}*/
			#endregion
			#region Footer
			listaNodos = reader.SelectNodes("data/Footer");
			XmlNode Footer;

			for (int i = 0; i < listaNodos.Count; i++)
			{
				Footer = listaNodos.Item(i);

				string CurrentFooterNode = Footer.SelectSingleNode("Note").InnerText;
				//Label1.Text = Label1.Text + CurrentFooterNode + "<br/>";
				QuestionnaireSection sssection = new QuestionnaireSection() { OrderInInstrument = 3, Questionnaire = proinstrument };
				proinstrument.Sections.Add(sssection);
				AddTextToSection(sssection, "Footer", CurrentFooterNode, Platform.Chat, Instance.Baseline | Instance.Followup);
			}
			#endregion

			return proinstrument;

		}
		public ProInstrument CreateOESInstrument()
		{
			ProInstrument pro = new ProInstrument();
			pro.Name = "OES2";
			pro.DisplayName = "OES";
			pro.Status = QuestionnaireStatus.Indevelopment;
			pro.IsActive = true;
			pro.DefaultFormatName = "OESStandardFormat";

			pro.Tags.Add(new Tag() { TagName = "Gender", Value = "Male" });
			pro.Tags.Add(new Tag() { TagName = "Gender", Value = "Female" });
			pro.Descriptions.Add(new QuestionnaireDescription() { Audience = UserTypes.All, Text = "Define description for this PRO" });
			pro.IntroductionMessages.Add(new QuestionnaireIntroductionMessage() { SupportedInstances = Instance.Baseline | Instance.Followup, SupportedPlatforms = Platform.Chat | Platform.Classic | Platform.Mobile, Text = "This Questionnaire will score your elbow functionality." });

			{
				ProDomain d1 = new ProDomain();
				d1.Name = "Total Domain";
				d1.Audience = UserTypes.Patient;
				d1.Description = "This shows the total score of all questions";
				d1.IsTotalDomain = true;
				d1.HigherIsBetter = true;
				d1.ScoringNote = "0 is worst - 100 is best";
				d1.ScoreFormula = "100/48*({OES.1} + {OES.2} + {OES.3} + {OES.4} + {OES.5} + {OES.6} + {OES.7} + {OES.8} + {OES.9} + {OES.10} + {OES.11} + {OES.12})";
				{
					ProDomainResultRange r1 = new ProDomainResultRange();
					r1.Start = 0;
					r1.End = 10;
					r1.Meaning = "Oops";
					d1.ResultRanges.Add(r1);
				}
				{
					ProDomainResultRange r2 = new ProDomainResultRange();
					r2.Start = 11;
					r2.End = 20;
					r2.Meaning = "Great";
					d1.ResultRanges.Add(r2);
				}
				pro.Domains.Add(d1);
			}

			{
				ProDomain d2 = new ProDomain();
				d2.Name = "Pain Domain";
				d2.Audience = UserTypes.Physician;
				d2.Description = "Describes the level of pain experienced.";
				d2.HigherIsBetter = true;
				d2.ScoringNote = "0 is worst - 100 is best";
				d2.ScoreFormula = "100/16*({OES.7} + {OES.8} + {OES.12} + {OES.11})";
				{
					ProDomainResultRange r1 = new ProDomainResultRange();
					r1.Start = 50;
					r1.End = 100;
					r1.Meaning = "Great";
					d2.ResultRanges.Add(r1);
				}
				{
					ProDomainResultRange r2 = new ProDomainResultRange();
					r2.Start = 0;
					r2.End = 49;
					r2.Meaning = "Not so great";
					d2.ResultRanges.Add(r2);
				}
				pro.Domains.Add(d2);
			}

			{
				ProDomain d2 = new ProDomain();
				d2.Name = "Elbow function";
				d2.Audience = UserTypes.Physician;
				d2.Description = "Indicates any limitation to elbow functionality";
				d2.HigherIsBetter = true;
				d2.ScoringNote = "0 is worst - 100 is best";
				d2.ScoreFormula = "100/16*({OES.4} + {OES.3} + {OES.1} + {OES.2})";
				{
					ProDomainResultRange r1 = new ProDomainResultRange();
					r1.Start = 50;
					r1.End = 100;
					r1.Meaning = "Great";
					d2.ResultRanges.Add(r1);
				}

				{
					ProDomainResultRange r2 = new ProDomainResultRange();
					r2.Start = 0;
					r2.End = 49;
					r2.Meaning = "Not so great";
					d2.ResultRanges.Add(r2);
				}
				pro.Domains.Add(d2);
			}

			{
				ProDomain d2 = new ProDomain();
				d2.Name = "Social-Psychological Domain";
				d2.Audience = UserTypes.Physician;
				d2.Description = "Brings forth any social-psychological issues encountered.";
				d2.HigherIsBetter = true;
				d2.ScoringNote = "0 is worst - 100 is best";
				d2.ScoreFormula = "100/16*({OES.10} + {OES.6} + {OES.5} + {OES.9})";
				{
					ProDomainResultRange r1 = new ProDomainResultRange();
					r1.Start = 50;
					r1.End = 100;
					r1.Meaning = "Great";
					d2.ResultRanges.Add(r1);
				}

				{
					ProDomainResultRange r2 = new ProDomainResultRange();
					r2.Start = 0;
					r2.End = 49;
					r2.Meaning = "Not so great";
					d2.ResultRanges.Add(r2);
				}
				pro.Domains.Add(d2);
			}

			pro.Concept = new QuestionnaireConcept() { Name = "Elbow", Description = "Tests Elbow Condition" };
			{
				QuestionnaireSection section = new QuestionnaireSection() { OrderInInstrument = 1, Questionnaire = pro };
				pro.Sections.Add(section);
				AddTextToSection(section, "Intro1", @"", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
				AddTextToSection(section, "Intro2", @"Please make sure you answer all the questions that follow by ticking one option for every question.", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
			}

			{
				//Dictionary<int, string> options = null;
				//var options;// = new MyDictionary();

				QuestionnaireSection section = new QuestionnaireSection() { OrderInInstrument = 2, Questionnaire = pro };
				pro.Sections.Add(section);
				{
					//Define the dictionary with the options for the optionGroups 
					MyDictionary options = new MyDictionary() { { 4, new ValuesOption() { Action = "", Text = "No difficulty" } }, { 3, new ValuesOption() { Action = "", Text = "A little bit of difficulty" } }, { 2, new ValuesOption() { Action = "", Text = "Moderate difficulty" } }, { 1, new ValuesOption() { Action = "", Text = "Extreme difficulty" } }, { 0, new ValuesOption() { Action = "", Text = "Impossible to do" } } };

					QuestionnaireItem qi = AddItemToSection(section, "OES.1", "1.", @"<strong>During the past 4 weeks...</strong><br />Have you had difficulty lifting things in your home, such as putting out the rubbish, <u>because of your elbow problem</u>?", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
					AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

					qi = AddItemToSection(section, "OES.2", "2.", @"<strong>During the past 4 weeks...</strong><br /> have you had difficulty carrying bags of shopping, <u>because of your elbow problem</u>?", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
					AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

					qi = AddItemToSection(section, "OES.3", "3.", @"<strong>During the past 4 weeks...</strong><br /> have you had difficulty washing yourself <u>all over</u>, <u>because of your elbow problem</u>?", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
					AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

					qi = AddItemToSection(section, "OES.4", "4.", @"<strong>During the past 4 weeks...</strong><br /> have you had difficulty dressing yourself, <u>because of your elbow problem</u>?", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
					AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

					qi = AddItemToSection(section, "OES.5", "5.", @"<strong>During the past 4 weeks...</strong><br /> have you felt that your elbow problem is “controlling your life”?", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
					options = new MyDictionary() { { 4, new ValuesOption() { Action = "", Text = "No, not at all" } }, { 3, new ValuesOption() { Action = "", Text = "Occasionally" } }, { 2, new ValuesOption() { Action = "", Text = "Some days" } }, { 1, new ValuesOption() { Action = "", Text = "Most days" } }, { 0, new ValuesOption() { Action = "", Text = "Every day" } } };
					AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

					qi = AddItemToSection(section, "OES.6", "6.", @"<strong>During the past 4 weeks...</strong><br /> How much has your elbow problem been ""on your mind""?", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
					options = new MyDictionary() { { 4, new ValuesOption() { Action = "", Text = "Not at all" } }, { 3, new ValuesOption() { Action = "", Text = "A little of the time" } }, { 2, new ValuesOption() { Action = "", Text = "Some of the time" } }, { 1, new ValuesOption() { Action = "", Text = "Most of the time" } }, { 0, new ValuesOption() { Action = "", Text = "All of the time" } } };
					AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

					qi = AddItemToSection(section, "OES.7", "7.", @"<strong>During the past 4 weeks...</strong><br />Have you been troubled by pain from your elbow in bed at night?", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
					options = new MyDictionary() { { 4, new ValuesOption() { Action = "", Text = "Not at all" } }, { 3, new ValuesOption() { Action = "", Text = "1 or 2 nights" } }, { 2, new ValuesOption() { Action = "", Text = "Some nights" } }, { 1, new ValuesOption() { Action = "", Text = "Most nights" } }, { 0, new ValuesOption() { Action = "", Text = "Every night" } } };
					AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

					qi = AddItemToSection(section, "OES.8", "8.", @"<strong>During the past 4 weeks...</strong><br />How often has your elbow pain interfered with your sleeping?", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
					options = new MyDictionary() { { 4, new ValuesOption() { Action = "", Text = "Not at all" } }, { 3, new ValuesOption() { Action = "", Text = "Occasionally" } }, { 2, new ValuesOption() { Action = "", Text = "Some of the time" } }, { 1, new ValuesOption() { Action = "", Text = "Most of the time" } }, { 0, new ValuesOption() { Action = "", Text = "All of the time" } } };
					AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

					qi = AddItemToSection(section, "OES.9", "9.", @"<strong>During the past 4 weeks...</strong><br />How much has your elbow problem interfered with your usual work or everyday activities?", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
					options = new MyDictionary() { { 4, new ValuesOption() { Action = "", Text = "Not at all" } }, { 3, new ValuesOption() { Action = "", Text = "A little bit" } }, { 2, new ValuesOption() { Action = "", Text = "Moderately" } }, { 1, new ValuesOption() { Action = "", Text = "Greately" } }, { 0, new ValuesOption() { Action = "", Text = "Totally" } } };
					AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

					qi = AddItemToSection(section, "OES.10", "10.", @"<strong>During the past 4 weeks...</strong><br />Has your elbow problem limited your ability to take part in leisure activities that you enjoy doing?", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
					options = new MyDictionary() { { 4, new ValuesOption() { Action = "", Text = "No, not at all" } }, { 3, new ValuesOption() { Action = "", Text = "Occasionally" } }, { 2, new ValuesOption() { Action = "", Text = "Some of the time" } }, { 1, new ValuesOption() { Action = "", Text = "Most of the time" } }, { 0, new ValuesOption() { Action = "", Text = "All of the time" } } };
					AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

					qi = AddItemToSection(section, "OES.11", "11.", @"<strong>During the past 4 weeks...</strong><br />How would you describe the <u>worst pain</u> you have from your elbow?", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
					options = new MyDictionary() { { 4, new ValuesOption() { Action = "", Text = "No pain" } }, { 3, new ValuesOption() { Action = "", Text = "Mild pain" } }, { 2, new ValuesOption() { Action = "", Text = "Moderate pain" } }, { 1, new ValuesOption() { Action = "", Text = "Severe pain" } }, { 0, new ValuesOption() { Action = "", Text = "Unbearable" } } };
					AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

					qi = AddItemToSection(section, "OES.12", "12.", @"<strong>During the past 4 weeks...</strong><br />How would you describe the pain you <u>usually</u> have from your elbow?", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
					AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.List);

					//adding the definition for the body control
					qi = AddItemToSection(section, "OES.13", "13.", @"Select the parts of your body with some kind of mal-functioning", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
					addBodyControl(section, qi);

					//options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "headFront" } }, { 1, new ValuesOption() { Action = "", Text = "leftHandFront" } }, { 2, new ValuesOption() { Action = "", Text = "leftForearmFront" } }, { 3, new ValuesOption() { Action = "", Text = "rightFootFront" } }, { 4, new ValuesOption() { Action = "", Text = "leftFootFront" } }, { 5, new ValuesOption() { Action = "", Text = "rightHandFront" } }, { 6, new ValuesOption() { Action = "", Text = "rightForearmFront" } }, { 7, new ValuesOption() { Action = "", Text = "rightLegFront" } }, { 8, new ValuesOption() { Action = "", Text = "leftLegFront" } }, { 9, new ValuesOption() { Action = "", Text = "rightArmFront" } }, { 10, new ValuesOption() { Action = "", Text = "leftArmFront" } }, { 11, new ValuesOption() { Action = "", Text = "neckFront" } }, { 12, new ValuesOption() { Action = "", Text = "chestFront" } }, { 13, new ValuesOption() { Action = "", Text = "stomachFront" } }, { 14, new ValuesOption() { Action = "", Text = "hipFront" } } };
					//AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.MultiSelect);

					//qi = AddItemToSection(section, "OES.6", "6.", @"How much trouble do you have with sexual activity because of your hip?", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
					//options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "This is not relevant to me" } } };
					//AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.ConditionalItem);
					//options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Several trouble" } }, { 100, new ValuesOption() { Action = "", Text = "No trouble at all" } } };
					//AddOptionGroupToItem(qi, options, 10, QuestionnaireResponseType.Range);

					//qi = AddItemToSection(section, "OES.7", "7.", @"How much trouble do you have pushing, pulling, lifting or carrying heavy objects at work?", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
					//options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "I do not do these actions in my activities" } } };
					//AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.ConditionalItem);
					//options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Several trouble" } }, { 100, new ValuesOption() { Action = "", Text = "No trouble at all" } } };
					//AddOptionGroupToItem(qi, options, 10, QuestionnaireResponseType.Range);

					//qi = AddItemToSection(section, "OES.8", "8.", @"How concern are you about cutting/changing directions during your sport or recreational activities?", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
					//options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "I do not do this action in my activities" } } };
					//AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.ConditionalItem);
					//options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "Extremly concerned" } }, { 100, new ValuesOption() { Action = "", Text = "Not concerned at all" } } };
					//AddOptionGroupToItem(qi, options, 10, QuestionnaireResponseType.Range);

					//qi = AddItemToSection(section, "OES.9", "9.", @"Please indicate the sport or instrument which is most important to you:", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
					//options = new MyDictionary() { { 0, new ValuesOption() { Action = "", Text = "" } } };
					//AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text);

					//qi = AddItemToSection(section, "OES.10", "10.", @"Enter your comments below:", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
					//AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text);

					/*
					qiog1.OrderInItem = 1;
					{
						qiog1.Options.Add(new QuestionnaireItemOption() { Text = "None", Value = 0, Group = qiog1 });
						qiog1.Options.Add(new QuestionnaireItemOption() { Text = "Mild", Value = 1, Group = qiog1 });
						qiog1.Options.Add(new QuestionnaireItemOption() { Text = "Moderate", Value = 2, Group = qiog1 });
						qiog1.Options.Add(new QuestionnaireItemOption() { Text = "Severe", Value = 3, Group = qiog1 });
						qiog1.Options.Add(new QuestionnaireItemOption() { Text = "Unbearable", Value = 4, Group = qiog1 });
						qiog1.Item = q1;
					}

					q1.OptionGroups.Add(qiog1);
					section.Elements.Add(q1);
					q1.Section = section;
					 * */
				}

			}

			{
				QuestionnaireSection section = new QuestionnaireSection() { OrderInInstrument = 3, Questionnaire = pro };
				pro.Sections.Add(section);
				AddTextToSection(section, "Footer1", @"Thank you for answering. You are done now and the results will be reported to your physician.", Platform.Classic, Instance.Baseline | Instance.Followup);
				AddTextToSection(section, "Footer1", @"Thank you for answering. You are done now and I will evaluate the results as soon as possible.", Platform.Chat, Instance.Baseline | Instance.Followup);
			}

			return pro;
		}
		public void FirstAppointmentQuestionnaire()
		{
			Survey pro = this.CreateFirstAppointmentQuestionnaire();
			OperationResult result = this.questionnaireClient.SaveFullQuestionnaire(pro);
		}
		public Survey CreateCurrentConditionQuestionnaire()
		{

			Survey pro = new Survey();
			pro.Name = "OPSMCCurrentCondition";
			pro.DisplayName = "Current Condition";
			pro.Status = QuestionnaireStatus.Indevelopment;
			pro.IsActive = true;
			pro.DefaultFormatName = "OPSMCCurrentConditionFormat";

			pro.Tags.Add(new Tag() { TagName = "Gender", Value = "Male" });
			pro.Tags.Add(new Tag() { TagName = "Gender", Value = "Female" });

			pro.Concept = new QuestionnaireConcept() { Name = "FirstAppointment", Description = "OPSMC First Appointment Questionnaire" };

			QuestionnaireSection section = new QuestionnaireSection() { OrderInInstrument = 1, Questionnaire = pro };
			pro.Sections.Add(section);
			AddTextToSection(section, "Intro.1", @"", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
			AddTextToSection(section, "Intro.2", @"PRESENT COMPLAINT", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);

			QuestionnaireItem qi = AddItemToSection(section, "OPSMC.CC.1", ".", @"We’d like to know more about the issue that brought you to OPSMC in the first place. 
								On the body diagram below, please tick the part(s) of the body that are affected by your current condition. Then tell us what kinds of sensations you’re 
								experiencing as a result of your condition. Select as many areas and sensations as needed. "
							, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
			addBodyControl(section, qi, "GOTO OPSMC.CC.2");

			qi = AddItemToSection(section, "OPSMC.CC.2", ".", @"How would you rate your average pain level? Click on the following scale, 0% means no pain and 100% means the worst
								possible pain. "
							, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
			MyDictionary options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.CC.3", Text = "No pain" } }, { 100, new ValuesOption() { Action = "GOTO OPSMC.CC.3", Text = "Worst possible pain" } } };
			AddOptionGroupToItem(qi, options, 10, QuestionnaireResponseType.Range);

			qi = AddItemToSection(section, "OPSMC.CC.3", ".", @"Have you had the same or a similar condition before? If so, please tell us what you had and when. "
							, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
			options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "No, I haven't" } }, { 1, new ValuesOption() { Action = "MakeItemApplicable", Text = "Yes" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.ConditionalItem);
			options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO OPSMC.CC.4", Text = "Add conditions" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text, "I had ");

			qi = AddItemToSection(section, "OPSMC.CC.4", ".", @"Before we move on, is there anything else we should know that might help us take better care of you? "
							, Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
			options = new MyDictionary() { { 0, new ValuesOption() { Action = "MakeItemNotApplicable", Text = "No, that’s all" } }, { 1, new ValuesOption() { Action = "MakeItemApplicable", Text = "Yes" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.ConditionalItem);
			options = new MyDictionary() { { 0, new ValuesOption() { Action = "GOTO END", Text = "Indicate aditional relevant information" } } };
			AddOptionGroupToItem(qi, options, 0, QuestionnaireResponseType.Text);

			return pro;
		}
		public void CurrentConditionQuestionnaire()
		{
			Survey pro = this.CreateCurrentConditionQuestionnaire();
			OperationResult result = this.questionnaireClient.SaveFullQuestionnaire(pro);
		}
		public void AddFullProInstrumentTest()
		{
			ProInstrument proOES = this.CreateOESInstrument();
			this.questionnaireClient.SaveFullQuestionnaire(proOES);

			ProInstrument proOESTest = this.CreateOESInstrument();
			proOESTest.Name = "Tester";
            proOESTest.DisplayName = "Tester";
			proOESTest.Tags.Add(new Tag() { TagName = "Test", Value = "Pedro" });
			this.questionnaireClient.SaveFullQuestionnaire(proOESTest);
		}
		public void RetrieveCompleteProInstrumentTest()
		{
			Questionnaire instrument = this.proClient.GetFullProInstrumentByName("OES2").Questionnaire;
		}
		public void AddQuestionnaireFormatDefinition()
		{
			{
				ContainerFormatDefinition canvasFormatDef = new ContainerFormatDefinition();
				canvasFormatDef.ContainerDefinitionName = "GenericQuestionnaireCanvas";
				canvasFormatDef.StartHtml = "<table>";
				canvasFormatDef.EndHtml = "</table>";
				canvasFormatDef.StartEndRepeat = 0;

				ItemFormatDefinition rowFormatDefn = new ItemFormatDefinition();
				rowFormatDefn.ElementFormatDefinitionName = "GenericQuestionnaireItem";
				rowFormatDefn.Html = "<tr><td><%DisplayId%/><%Text%/></td></tr>";
				rowFormatDefn.ContainerFormatDefinition = canvasFormatDef;
				canvasFormatDef.ElementFormatDefinitions.Add(rowFormatDefn);

				{
					//Horizontal radio element
					ItemGroupOptionsFormatDefinition horizontalRadioFormatDef = new ItemGroupOptionsFormatDefinition();
					rowFormatDefn.ItemGroupOptionsFormatDefinitions.Add(horizontalRadioFormatDef);
					horizontalRadioFormatDef.GroupOptionDefinitionName = "LikertHorizontalRadio";
					horizontalRadioFormatDef.StartHtml = "<tr><td><%OptionGroupText%/><table>";
					horizontalRadioFormatDef.EndHtml = "</table></td></tr>";
					horizontalRadioFormatDef.ForEachOptionStart = "<tr>";
					horizontalRadioFormatDef.ForEachOptionEnd = "</tr>";
					horizontalRadioFormatDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td><%OptionText%/></td>" });
					horizontalRadioFormatDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td>", ItemOptionDisplayType = ItemOptionDisplayType.RadioButton, EndText = "</td>" });
				}
				
				{
					//Vertical Radio element
					ItemGroupOptionsFormatDefinition verticalRadioFormatDef = new ItemGroupOptionsFormatDefinition();
					rowFormatDefn.ItemGroupOptionsFormatDefinitions.Add(verticalRadioFormatDef);
					verticalRadioFormatDef.GroupOptionDefinitionName = "LikertVerticalRadio";
					verticalRadioFormatDef.StartHtml = "<tr><td><%OptionGroupText%/><table>";
					verticalRadioFormatDef.EndHtml = "</table></td></tr>";
					verticalRadioFormatDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<tr><td>", ItemOptionDisplayType = ItemOptionDisplayType.RadioButton, EndText = "</td><td><%OptionText%/></td></tr>" });
				}

				{
					//Dropdown element
					ItemGroupOptionsFormatDefinition DropDownFormatDef = new ItemGroupOptionsFormatDefinition();
					rowFormatDefn.ItemGroupOptionsFormatDefinitions.Add(DropDownFormatDef);
					DropDownFormatDef.GroupOptionDefinitionName = "DropDown";
					DropDownFormatDef.StartHtml = "<tr><td><%OptionGroupText%/><table>";
					DropDownFormatDef.EndHtml = "</table></td></tr>";
					DropDownFormatDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { ItemOptionDisplayType = ItemOptionDisplayType.DropDown });
				}

				{
					//Slider Element
					ItemFormatDefinition rowSliderFormatDefn = new ItemFormatDefinition();
					rowSliderFormatDefn.ElementFormatDefinitionName = "SliderQuestionnaireItem";
					rowSliderFormatDefn.Html = "<tr><td><%DisplayId%/><%Text%/></td></tr>";
					rowSliderFormatDefn.ContainerFormatDefinition = canvasFormatDef;
					canvasFormatDef.ElementFormatDefinitions.Add(rowSliderFormatDefn);

					ItemGroupOptionsFormatDefinition sliderDef = new ItemGroupOptionsFormatDefinition();
					rowSliderFormatDefn.ItemGroupOptionsFormatDefinitions.Add(sliderDef);
					sliderDef.GroupOptionDefinitionName = "LikertSlider";
					sliderDef.StartHtml = "<tr><td><%OptionGroupText%/><table>";
					sliderDef.EndHtml = "</table></td></tr>";
					sliderDef.ForEachOptionStart = "<tr>";
					sliderDef.ForEachOptionEnd = "</tr>";
					sliderDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td><%OptionText%/></td><td>", ItemOptionDisplayType = ItemOptionDisplayType.Slider, EndText = "</td><td><%OptionText%/></td>" });
				}

				{
					//Conditional Item
					ItemFormatDefinition conditionalRowFormatDefn = new ItemFormatDefinition();
					conditionalRowFormatDefn.ElementFormatDefinitionName = "ConditionalQuestionnaireItem";
					conditionalRowFormatDefn.Html = "<tr><td><%DisplayId%/><%Text%/></td></tr>";
					conditionalRowFormatDefn.ContainerFormatDefinition = canvasFormatDef;
					canvasFormatDef.ElementFormatDefinitions.Add(conditionalRowFormatDefn);

					ItemGroupOptionsFormatDefinition conditionalItemDef = new ItemGroupOptionsFormatDefinition();
					conditionalRowFormatDefn.ItemGroupOptionsFormatDefinitions.Add(conditionalItemDef);
					conditionalItemDef.GroupOptionDefinitionName = "ConditionalItem";
					conditionalItemDef.StartHtml = "<tr><td><%OptionGroupText%/><table>";
					conditionalItemDef.EndHtml = "</table></td></tr>";
					conditionalItemDef.ForEachOptionStart = "<tr>";
					conditionalItemDef.ForEachOptionEnd = "</tr>";
					conditionalItemDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td>", EndText = "</td><td><%OptionText%/></td>" });
				}

				{
					//Textbox element
					ItemFormatDefinition rowTextBoxFormatDefn = new ItemFormatDefinition();
					rowTextBoxFormatDefn.ElementFormatDefinitionName = "TextBoxQuestionnaireItem";
					rowTextBoxFormatDefn.Html = "<tr><td><%DisplayId%/><%Text%/></td></tr>";
					rowTextBoxFormatDefn.ContainerFormatDefinition = canvasFormatDef;
					canvasFormatDef.ElementFormatDefinitions.Add(rowTextBoxFormatDefn);

					ItemGroupOptionsFormatDefinition textBoxDef = new ItemGroupOptionsFormatDefinition();
					rowTextBoxFormatDefn.ItemGroupOptionsFormatDefinitions.Add(textBoxDef);
					textBoxDef.GroupOptionDefinitionName = "TextBox";
					textBoxDef.StartHtml = "<tr><td><%OptionGroupText%/><table>";
					textBoxDef.EndHtml = "</table></td></tr>";
					textBoxDef.ForEachOptionStart = "<tr>";
					textBoxDef.ForEachOptionEnd = "</tr>";
					textBoxDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td>", ItemOptionDisplayType = ItemOptionDisplayType.TextBox, EndText = "</td>" });
				}

				{
					//Textarea element
					ItemFormatDefinition rowTextAreaFormatDefn = new ItemFormatDefinition();
					rowTextAreaFormatDefn.ElementFormatDefinitionName = "TextAreaQuestionnaireItem";
					rowTextAreaFormatDefn.Html = "<tr><td><%DisplayId%/><%Text%/></td></tr>";
					rowTextAreaFormatDefn.ContainerFormatDefinition = canvasFormatDef;
					canvasFormatDef.ElementFormatDefinitions.Add(rowTextAreaFormatDefn);

					ItemGroupOptionsFormatDefinition textAreaDef = new ItemGroupOptionsFormatDefinition();
					rowTextAreaFormatDefn.ItemGroupOptionsFormatDefinitions.Add(textAreaDef);
					textAreaDef.GroupOptionDefinitionName = "TextArea";
					textAreaDef.StartHtml = "<tr><td><%OptionGroupText%/><table>";
					textAreaDef.EndHtml = "</table></td></tr>";
					textAreaDef.ForEachOptionStart = "<tr>";
					textAreaDef.ForEachOptionEnd = "</tr>";
					textAreaDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td>", ItemOptionDisplayType = ItemOptionDisplayType.TextArea, EndText = "</td>" });
				}

				{
					//Horizontal checkbox element
					ItemFormatDefinition rowHorizontalCheckboxFormatDefn = new ItemFormatDefinition();
					rowHorizontalCheckboxFormatDefn.ElementFormatDefinitionName = "HorizontalCheckboxQuestionnaireItem";
					rowHorizontalCheckboxFormatDefn.Html = "<tr><td><%DisplayId%/><%Text%/></td></tr>";
					rowHorizontalCheckboxFormatDefn.ContainerFormatDefinition = canvasFormatDef;
					canvasFormatDef.ElementFormatDefinitions.Add(rowHorizontalCheckboxFormatDefn);

					ItemGroupOptionsFormatDefinition HorizontalCheckboxDef = new ItemGroupOptionsFormatDefinition();
					rowHorizontalCheckboxFormatDefn.ItemGroupOptionsFormatDefinitions.Add(HorizontalCheckboxDef);
					HorizontalCheckboxDef.GroupOptionDefinitionName = "LikertHorizontalCheckbox";
					HorizontalCheckboxDef.StartHtml = "<tr><td><%OptionGroupText%/><table>";
					HorizontalCheckboxDef.EndHtml = "</table></td></tr>";
					HorizontalCheckboxDef.ForEachOptionStart = "<tr>";
					HorizontalCheckboxDef.ForEachOptionEnd = "</tr>";
					HorizontalCheckboxDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td><%OptionText%/></td>" });
					HorizontalCheckboxDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td>", ItemOptionDisplayType = ItemOptionDisplayType.CheckBox, EndText = "</td>" });
				}

				{
					//Vertical checkbox element
					ItemFormatDefinition rowVerticalCheckboxFormatDefn = new ItemFormatDefinition();
					rowVerticalCheckboxFormatDefn.ElementFormatDefinitionName = "VerticalCheckboxQuestionnaireItem";
					rowVerticalCheckboxFormatDefn.Html = "<tr><td><%DisplayId%/><%Text%/></td></tr>";
					rowVerticalCheckboxFormatDefn.ContainerFormatDefinition = canvasFormatDef;
					canvasFormatDef.ElementFormatDefinitions.Add(rowVerticalCheckboxFormatDefn);

					ItemGroupOptionsFormatDefinition VerticalCheckboxDef = new ItemGroupOptionsFormatDefinition();
					rowVerticalCheckboxFormatDefn.ItemGroupOptionsFormatDefinitions.Add(VerticalCheckboxDef);
					VerticalCheckboxDef.GroupOptionDefinitionName = "LikertVerticalCheckbox";
					VerticalCheckboxDef.StartHtml = "<tr><td><%OptionGroupText%/><table>";
					VerticalCheckboxDef.EndHtml = "</table></td></tr>";
					VerticalCheckboxDef.ForEachOptionStart = "<tr>";
					VerticalCheckboxDef.ForEachOptionEnd = "</tr>";
					VerticalCheckboxDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td><label>", ItemOptionDisplayType = ItemOptionDisplayType.CheckBox, EndText = "<%OptionText%/></label></td>" });
				}

				{
					//Datepicker element
					ItemFormatDefinition rowDatePickerFormatDefn = new ItemFormatDefinition();
					rowDatePickerFormatDefn.ElementFormatDefinitionName = "DatePickerQuestionnaireItem";
					rowDatePickerFormatDefn.Html = "<tr><td><%DisplayId%/><%Text%/></td></tr>";
					rowDatePickerFormatDefn.ContainerFormatDefinition = canvasFormatDef;
					canvasFormatDef.ElementFormatDefinitions.Add(rowDatePickerFormatDefn);

					ItemGroupOptionsFormatDefinition DatePickerDef = new ItemGroupOptionsFormatDefinition();
					rowDatePickerFormatDefn.ItemGroupOptionsFormatDefinitions.Add(DatePickerDef);
					DatePickerDef.GroupOptionDefinitionName = "DatePicker";
					DatePickerDef.StartHtml = "<tr><td><%OptionGroupText%/><table>";
					DatePickerDef.EndHtml = "</table></td></tr>";
					DatePickerDef.ForEachOptionStart = "<tr>";
					DatePickerDef.ForEachOptionEnd = "</tr>";
					DatePickerDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td>", ItemOptionDisplayType = ItemOptionDisplayType.DatePicker, EndText = "</td>" });
				}

				{
					//Body chart element
					//When defining the body chart the tags <%BodyControl%/> must be in order to indicate the renderer where the map tag and the options are going to be displayed
					ItemFormatDefinition rowBodyControlFormatDefn = new ItemFormatDefinition();
					rowBodyControlFormatDefn.ElementFormatDefinitionName = "BodyControlQuestionnaireItem";
					rowBodyControlFormatDefn.Html = "<tr><td><%DisplayId%/><%Text%/></td></tr><%BodyControl%/><tr><td><%BodyMap%/></td></tr><tr><td><%OptionsForBodyPart%/></td></tr><%BodyControl%/>";
					rowBodyControlFormatDefn.ContainerFormatDefinition = canvasFormatDef;
					canvasFormatDef.ElementFormatDefinitions.Add(rowBodyControlFormatDefn);

					ItemGroupOptionsFormatDefinition bodyControlDef = new ItemGroupOptionsFormatDefinition();
					rowBodyControlFormatDefn.ItemGroupOptionsFormatDefinitions.Add(bodyControlDef);
					bodyControlDef.GroupOptionDefinitionName = "BodyControl";
					bodyControlDef.StartHtml = "<tr  style=\"display: none;\"><td></td><td><table>";
					bodyControlDef.EndHtml = "</table></td></tr>";
					bodyControlDef.ForEachOptionStart = "<tr>";
					bodyControlDef.ForEachOptionEnd = "</tr>";
					bodyControlDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td>", ItemOptionDisplayType = ItemOptionDisplayType.HiddenCheckBox, EndText = "</td>" });
				}
				

				TextFormatDefinition textFormatDef = new TextFormatDefinition();
				textFormatDef.ElementFormatDefinitionName = "GenericQuestionnaireText";
				textFormatDef.Html = "<tr><td colspan=\"2\"><%Text%/></td></tr>";
				textFormatDef.ContainerFormatDefinition = canvasFormatDef;
				canvasFormatDef.ElementFormatDefinitions.Add(textFormatDef);

				this.questionnaireFormatClient.AddOrUpdateFullDefinitionContainer(canvasFormatDef);
			}
		}
		public void AddChatFormatDefinition()
		{
			ContainerFormatDefinition canvasFormatDef = new ContainerFormatDefinition();
			canvasFormatDef.ContainerDefinitionName = "GenericQuestionnaireChatCanvas";

			ItemFormatDefinition itemDefinition = new ItemFormatDefinition();
			itemDefinition.ElementFormatDefinitionName = "GenericChatItemDefinition";
			itemDefinition.ContainerFormatDefinition = canvasFormatDef;
			canvasFormatDef.ElementFormatDefinitions.Add(itemDefinition);
			itemDefinition.Html = "<p><%Text%/></p>";
			/*
			itemDefinition.StartHtml = @"<div id=""MainDiv.<%ActionId%/>"">";
			itemDefinition.Html = @"<div id=""DrDiv.<%ActionId%/>"" class=""Dr-Box"">
		<div class=""Dr-Msg""><%Text%/></div>
	</div>
	<div id=""UserDiv.<%ActionId%/>"" class=""User-Box"">
		<div id=""UserAnswerDiv.<%ActionId%/> class=""User-Msg"">
			<div class=""edit-Icon"">
				<a href=""#""><img src=""../content/images/btn-Edit.png"" width=""46"" height=""46"" alt=""""/></a>Edit
			</div>
			<div id=""UserAnswer.<%ActionId%/>""></div>
		</div>
	</div> 
	<div id=""UserResponse.<%ActionId%/>"" class=""Msg-Box"">
		<div class=""User-Avatar""></div>";
			itemDefinition.EndHtml = @"<div><input id=""MsgSend.<%ActionId%/>"" type=""submit"" class=""Msg-Send"" value=""Send""></div>
		<div class=""skipLink""><a href=""#"">skip it</a></div>
	</div>	
</div>";*/

			{
				//Body chart element
				//When defining the body chart the tags <%BodyControl%/> must be in order to indicate the renderer where the map tag and the options are going to be displayed
				ItemFormatDefinition ChatRowBodyControlFormatDefn = new ItemFormatDefinition();
				ChatRowBodyControlFormatDefn.ElementFormatDefinitionName = "ChatBodyControlQuestionnaireItem";
				ChatRowBodyControlFormatDefn.Html = "<tr><td><%Text%/></td></tr><%BodyControl%/><tr><td><%BodyMap%/></td></tr><tr><td><%OptionsForBodyPart%/></td></tr><%BodyControl%/>";
				ChatRowBodyControlFormatDefn.ContainerFormatDefinition = canvasFormatDef;
				ChatRowBodyControlFormatDefn.StartHtml = "<table>";
				ChatRowBodyControlFormatDefn.EndHtml = "</table>";
				canvasFormatDef.ElementFormatDefinitions.Add(ChatRowBodyControlFormatDefn);

				ItemGroupOptionsFormatDefinition ChatBodyControlDef = new ItemGroupOptionsFormatDefinition();
				ChatRowBodyControlFormatDefn.ItemGroupOptionsFormatDefinitions.Add(ChatBodyControlDef);
				ChatBodyControlDef.GroupOptionDefinitionName = "ChatBodyControl";
				ChatBodyControlDef.StartHtml = "<tr style=\"display: none;\"><td></td><td><table>";
				ChatBodyControlDef.EndHtml = "</table></td></tr>";
				ChatBodyControlDef.ForEachOptionStart = "<tr>";
				ChatBodyControlDef.ForEachOptionEnd = "</tr>";
				ChatBodyControlDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td>", ItemOptionDisplayType = ItemOptionDisplayType.HiddenCheckBox, EndText = "</td>" });

			}

			{
				ItemGroupOptionsFormatDefinition ChatLikertHorizontalRadio = new ItemGroupOptionsFormatDefinition();
				itemDefinition.ItemGroupOptionsFormatDefinitions.Add(ChatLikertHorizontalRadio);
				ChatLikertHorizontalRadio.GroupOptionDefinitionName = "ChatLikertHorizontalRadio";
				ChatLikertHorizontalRadio.StartHtml = "<table><tr><td><%OptionGroupText%/></td></tr>";
				ChatLikertHorizontalRadio.EndHtml = "</table>";
				ChatLikertHorizontalRadio.ForEachOptionStart = "<tr>";
				ChatLikertHorizontalRadio.ForEachOptionEnd = "</tr>";
				ChatLikertHorizontalRadio.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td><%OptionText%/></td>" });
				ChatLikertHorizontalRadio.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td>", ItemOptionDisplayType = ItemOptionDisplayType.RadioButton, EndText = "</td>" });
			}

			{
				ItemGroupOptionsFormatDefinition ChatLikertHorizontalCheckbox = new ItemGroupOptionsFormatDefinition();
				itemDefinition.ItemGroupOptionsFormatDefinitions.Add(ChatLikertHorizontalCheckbox);
				ChatLikertHorizontalCheckbox.GroupOptionDefinitionName = "ChatLikertHorizontalCheckbox";
				ChatLikertHorizontalCheckbox.StartHtml = "<table><tr><td><%OptionGroupText%/></td></tr>";
				ChatLikertHorizontalCheckbox.EndHtml = "</table>";
				ChatLikertHorizontalCheckbox.ForEachOptionStart = "<tr>";
				ChatLikertHorizontalCheckbox.ForEachOptionEnd = "</tr>";
				ChatLikertHorizontalCheckbox.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td><%OptionText%/></td>" });
				ChatLikertHorizontalCheckbox.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td>", ItemOptionDisplayType = ItemOptionDisplayType.CheckBox, EndText = "</td>" });
			}

			{
				ItemGroupOptionsFormatDefinition ChatLikertVerticalRadio = new ItemGroupOptionsFormatDefinition();
				itemDefinition.ItemGroupOptionsFormatDefinitions.Add(ChatLikertVerticalRadio);
				ChatLikertVerticalRadio.GroupOptionDefinitionName = "ChatLikertVerticalRadio";
				ChatLikertVerticalRadio.StartHtml = "<table><tr><td><%OptionGroupText%/></td></tr>";
				ChatLikertVerticalRadio.EndHtml = "</table>";
				ChatLikertVerticalRadio.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<tr><td><label>", ItemOptionDisplayType = ItemOptionDisplayType.RadioButton, EndText = "<%OptionText%/></label></td></tr>" });
			}

			{
				ItemGroupOptionsFormatDefinition ChatLikertVerticalCheckbox = new ItemGroupOptionsFormatDefinition();
				itemDefinition.ItemGroupOptionsFormatDefinitions.Add(ChatLikertVerticalCheckbox);
				ChatLikertVerticalCheckbox.GroupOptionDefinitionName = "ChatLikertVerticalCheckbox";
				ChatLikertVerticalCheckbox.StartHtml = "<table><tr><td><%OptionGroupText%/></td></tr>";
				ChatLikertVerticalCheckbox.EndHtml = "</table>";
				ChatLikertVerticalCheckbox.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<tr><td><label>", ItemOptionDisplayType = ItemOptionDisplayType.CheckBox, EndText = "<%OptionText%/></label></td></tr>" });
			}

			{
				ItemGroupOptionsFormatDefinition ChatSliderDef = new ItemGroupOptionsFormatDefinition();
				itemDefinition.ItemGroupOptionsFormatDefinitions.Add(ChatSliderDef);
				ChatSliderDef.GroupOptionDefinitionName = "ChatLikertSlider";
				ChatSliderDef.StartHtml = "<table><tr><td><%OptionGroupText%/></td></tr>";
				ChatSliderDef.EndHtml = "</table>";
				ChatSliderDef.ForEachOptionStart = "<tr>";
				ChatSliderDef.ForEachOptionEnd = "</tr>";
				ChatSliderDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td><%OptionText%/></td><td>", ItemOptionDisplayType = ItemOptionDisplayType.Slider, EndText = "</td><td><%OptionText%/></td>" });
			}

			{
				ItemGroupOptionsFormatDefinition ChatConditionalItemDef = new ItemGroupOptionsFormatDefinition();
				itemDefinition.ItemGroupOptionsFormatDefinitions.Add(ChatConditionalItemDef);
				ChatConditionalItemDef.GroupOptionDefinitionName = "ChatConditionalItem";
				ChatConditionalItemDef.StartHtml = "<table><tr><td><%OptionGroupText%/></td></tr>";
				ChatConditionalItemDef.EndHtml = "</table>";
				ChatConditionalItemDef.ForEachOptionStart = "<tr>";
				ChatConditionalItemDef.ForEachOptionEnd = "</tr>"; ChatConditionalItemDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td><label><%OptionText%/>", ItemOptionDisplayType = ItemOptionDisplayType.RadioButton, EndText = "</label></td>" });
			}

			{
				ItemGroupOptionsFormatDefinition ChatTextBoxDef = new ItemGroupOptionsFormatDefinition();
				itemDefinition.ItemGroupOptionsFormatDefinitions.Add(ChatTextBoxDef);
				ChatTextBoxDef.GroupOptionDefinitionName = "ChatTextBox";
				ChatTextBoxDef.StartHtml = "<table><tr><td><%OptionGroupText%/></td></tr>";
				ChatTextBoxDef.EndHtml = "</table>";
				ChatTextBoxDef.ForEachOptionStart = "<tr>";
				ChatTextBoxDef.ForEachOptionEnd = "</tr>";
				ChatTextBoxDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td>", ItemOptionDisplayType = ItemOptionDisplayType.TextBox, EndText = "</td>" });
			}

			{
				ItemGroupOptionsFormatDefinition ChatTextAreaDef = new ItemGroupOptionsFormatDefinition();
				itemDefinition.ItemGroupOptionsFormatDefinitions.Add(ChatTextAreaDef);
				ChatTextAreaDef.GroupOptionDefinitionName = "ChatTextArea";
				ChatTextAreaDef.StartHtml = "<table><tr><td><%OptionGroupText%/></td></tr>";
				ChatTextAreaDef.EndHtml = "</table>";
				ChatTextAreaDef.ForEachOptionStart = "<tr>";
				ChatTextAreaDef.ForEachOptionEnd = "</tr>";
				ChatTextAreaDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td>", ItemOptionDisplayType = ItemOptionDisplayType.TextArea, EndText = "</td>" });

			}

			{
				ItemGroupOptionsFormatDefinition ChatDatePickerDef = new ItemGroupOptionsFormatDefinition();
				itemDefinition.ItemGroupOptionsFormatDefinitions.Add(ChatDatePickerDef);
				ChatDatePickerDef.GroupOptionDefinitionName = "ChatDatePicker";
				ChatDatePickerDef.StartHtml = "<table><tr><td><%OptionGroupText%/></td></tr>";
				ChatDatePickerDef.EndHtml = "</table>";
				ChatDatePickerDef.ForEachOptionStart = "<tr>";
				ChatDatePickerDef.ForEachOptionEnd = "</tr>";
				ChatDatePickerDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { StartText = "<td>", ItemOptionDisplayType = ItemOptionDisplayType.DatePicker, EndText = "</td>" });
			}

			{
				ItemGroupOptionsFormatDefinition ChatDropdownDef = new ItemGroupOptionsFormatDefinition();
				itemDefinition.ItemGroupOptionsFormatDefinitions.Add(ChatDropdownDef);
				ChatDropdownDef.GroupOptionDefinitionName = "ChatDropdown";
				ChatDropdownDef.StartHtml = "<table><tr><td><%OptionGroupText%/></td>";
				ChatDropdownDef.EndHtml = "</tr></table>";
				ChatDropdownDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { ItemOptionDisplayType = ItemOptionDisplayType.DropDown });
			}

			{
				ItemGroupOptionsFormatDefinition ChatPasswordTextBoxDef = new ItemGroupOptionsFormatDefinition();
				itemDefinition.ItemGroupOptionsFormatDefinitions.Add(ChatPasswordTextBoxDef);
				ChatPasswordTextBoxDef.GroupOptionDefinitionName = "ChatPasswordTextBox";
				ChatPasswordTextBoxDef.StartHtml = "<table><tr><td><%OptionGroupText%/></td></tr>";
				ChatPasswordTextBoxDef.EndHtml = "</table>";
				ChatPasswordTextBoxDef.ForEachOptionStart = "<tr>";
				ChatPasswordTextBoxDef.ForEachOptionEnd = "</tr>";
				ChatPasswordTextBoxDef.ForEachOption.Add(new ItemGroupOptionsForEachOptionDefinition() { ItemOptionDisplayType = ItemOptionDisplayType.Password });
			}

			this.questionnaireFormatClient.AddOrUpdateFullDefinitionContainer(canvasFormatDef);
		}
		public void AddChatFormat()
		{
			Format format = new Format();
			format.Name = "OESStandardFormat";
			format.SupportedPlatform = Platform.Chat;
			{
				ItemFormatContainer items = new ItemFormatContainer();
				items.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericChatItemDefinition" };
				items.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatLikertHorizontalRadio" }, ResponseType = QuestionnaireResponseType.List });
				items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.1", OrderInSection = 1 });
				items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.2", OrderInSection = 2 });
				items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.3", OrderInSection = 3 });
				items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.4", OrderInSection = 4 });
				format.Containers.Add(items);
			}

			{
				ItemFormatContainer items = new ItemFormatContainer();
				//container.ContainerFormatDefinition = ""
				items.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericChatItemDefinition" };
				items.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatLikertVerticalRadio" }, ResponseType = QuestionnaireResponseType.List });
				items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.5", OrderInSection = 3 });
				items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.6", OrderInSection = 4 });
				items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.7", OrderInSection = 3 });
				items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.8", OrderInSection = 4 });
				items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.9", OrderInSection = 3 });
				items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.10", OrderInSection = 4 });
				items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.11", OrderInSection = 3 });
				items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.12", OrderInSection = 4 });
				format.Containers.Add(items);
			}


			{
				ItemFormatContainer itemsBodyControl = new ItemFormatContainer();
				//container.ContainerFormatDefinition = ""
				itemsBodyControl.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "ChatBodyControlQuestionnaireItem" };
				itemsBodyControl.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatBodyControl" }, ResponseType = QuestionnaireResponseType.MultiSelect });
				itemsBodyControl.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.CC.1", OrderInSection = 1 });
				format.Containers.Add(itemsBodyControl);
				//pro.Children.Add(itemsBodyControl);
			}
			/*
			{
				ItemFormatContainer items = new ItemFormatContainer();
				//container.ContainerFormatDefinition = ""
				items.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericChatItemDefinition" };
				items.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatLikertSlider" }, ResponseType = QuestionnaireResponseType.Range });
				items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.5", OrderInSection = 5 });
				format.Containers.Add(items);
			}

			{
				ItemFormatContainer items = new ItemFormatContainer();
				//container.ContainerFormatDefinition = ""
				items.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericChatItemDefinition" };
				//items.ItemFormatDefinition.ItemGroupOptionsFormatDefinitions = new ItemGroupOptionsFormatDefinition() { };
				items.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatConditionalItem" }, ResponseType = QuestionnaireResponseType.ConditionalItem });
				items.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatLikertSlider" }, ResponseType = QuestionnaireResponseType.Range });
				items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.6", OrderInSection = 6 });
				items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.7", OrderInSection = 7 });
				items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.8", OrderInSection = 8 });
				format.Containers.Add(items);
			}

			{
				ItemFormatContainer items = new ItemFormatContainer();
				//container.ContainerFormatDefinition = ""
				items.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericChatItemDefinition" };
				items.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatTextBox" }, ResponseType = QuestionnaireResponseType.Text });
				items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.9", OrderInSection = 9 });
				format.Containers.Add(items);
			}

			{
				ItemFormatContainer items = new ItemFormatContainer();
				//container.ContainerFormatDefinition = ""
				items.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericChatItemDefinition" };
				items.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatTextArea" }, ResponseType = QuestionnaireResponseType.Text });
				items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.10", OrderInSection = 10 });
				format.Containers.Add(items);
			}
			*/
			this.questionnaireFormatClient.AddOrUpdateFullFormat(format);
		}

		public void AddFirstAppointmentClassicFormat()
		{
			Format format = new Format();
			format.Name = "OPSMCFirstAppointmentFormat";
			format.SupportedPlatform = Platform.Classic;


			FormatContainer pro = new FormatContainer();
			format.Containers.Add(pro);
			pro.ContainerFormatDefinition = new ContainerFormatDefinition() { ContainerDefinitionName = "GenericQuestionnaireCanvas" };



			{
				ItemFormatContainer itemsConditionalItemDatePicker = new ItemFormatContainer();
				//container.ContainerFormatDefinition = ""
				itemsConditionalItemDatePicker.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericQuestionnaireItem" };
				itemsConditionalItemDatePicker.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "TextBox" }, ResponseType = QuestionnaireResponseType.Text });
				itemsConditionalItemDatePicker.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "DatePicker" }, ResponseType = QuestionnaireResponseType.List });
				itemsConditionalItemDatePicker.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ConditionalItem" }, ResponseType = QuestionnaireResponseType.ConditionalItem });
				itemsConditionalItemDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.15", OrderInSection = 15 });
				itemsConditionalItemDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.15.1", OrderInSection = 15 });
				itemsConditionalItemDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.37", OrderInSection = 37 });
				itemsConditionalItemDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.58", OrderInSection = 58 });
				pro.Children.Add(itemsConditionalItemDatePicker);
				//format.Containers.Add(itemsConditionalItemDatePicker);
			}

			{
				ItemFormatContainer itemsHorizontalRadio = new ItemFormatContainer();
				itemsHorizontalRadio.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericQuestionnaireItem" };
				itemsHorizontalRadio.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "LikertHorizontalRadio" }, ResponseType = QuestionnaireResponseType.List });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.20", OrderInSection = 20 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.40", OrderInSection = 40 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.42", OrderInSection = 42 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.43", OrderInSection = 43 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.44", OrderInSection = 44 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.45", OrderInSection = 45 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.46", OrderInSection = 46 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.48", OrderInSection = 48 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.49", OrderInSection = 49 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.60", OrderInSection = 60 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.62", OrderInSection = 62 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.64", OrderInSection = 64 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.69", OrderInSection = 69 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.70", OrderInSection = 1 });
				pro.Children.Add(itemsHorizontalRadio);
				//format.Containers.Add(itemsHorizontalRadio);
			}

			{
				ItemFormatContainer itemsBodyControl = new ItemFormatContainer();
				//container.ContainerFormatDefinition = ""
				itemsBodyControl.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "BodyControlQuestionnaireItem" };
				itemsBodyControl.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "BodyControl" }, ResponseType = QuestionnaireResponseType.MultiSelect });
				itemsBodyControl.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.54", OrderInSection = 54 });
				//format.Containers.Add(itemsBodyControl);
				pro.Children.Add(itemsBodyControl);
			}

			{
				ItemFormatContainer itemsDropdown = new ItemFormatContainer();
				itemsDropdown.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericQuestionnaireItem" };
				itemsDropdown.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "Dropdown" }, ResponseType = QuestionnaireResponseType.List });
				itemsDropdown.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "TextBox" }, ResponseType = QuestionnaireResponseType.Text });
				itemsDropdown.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ConditionalItem" }, ResponseType = QuestionnaireResponseType.ConditionalItem });
				itemsDropdown.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.19", OrderInSection = 19 });
				itemsDropdown.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.59", OrderInSection = 59 });
				itemsDropdown.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.67", OrderInSection = 67 });
				pro.Children.Add(itemsDropdown);
				//format.Containers.Add(itemsDropdown);
			}

			{
				ItemFormatContainer itemsConditionalItem = new ItemFormatContainer();
				itemsConditionalItem.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericQuestionnaireItem" };
				itemsConditionalItem.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "TextBox" }, ResponseType = QuestionnaireResponseType.Text });
				itemsConditionalItem.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "LikertHorizontalRadio" }, ResponseType = QuestionnaireResponseType.List });
				itemsConditionalItem.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ConditionalItem" }, ResponseType = QuestionnaireResponseType.ConditionalItem });
				itemsConditionalItem.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.18", OrderInSection = 18 });
				itemsConditionalItem.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.36", OrderInSection = 36 });
				itemsConditionalItem.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.38", OrderInSection = 38 });
				itemsConditionalItem.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.50", OrderInSection = 50 });
				itemsConditionalItem.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.51", OrderInSection = 51 });
				itemsConditionalItem.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.52", OrderInSection = 52 });
				itemsConditionalItem.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.53", OrderInSection = 53 });
				itemsConditionalItem.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.56", OrderInSection = 56 });
				itemsConditionalItem.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.68", OrderInSection = 68 });
				pro.Children.Add(itemsConditionalItem);
				//format.Containers.Add(itemsConditionalItem);
			}

			{
				ItemFormatContainer itemsConditionalItemTextArea = new ItemFormatContainer();
				itemsConditionalItemTextArea.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericQuestionnaireItem" };
				itemsConditionalItemTextArea.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "TextArea" }, ResponseType = QuestionnaireResponseType.Text });
				itemsConditionalItemTextArea.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "LikertHorizontalRadio" }, ResponseType = QuestionnaireResponseType.List });
				itemsConditionalItemTextArea.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ConditionalItem" }, ResponseType = QuestionnaireResponseType.ConditionalItem });
				itemsConditionalItemTextArea.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.57", OrderInSection = 57 });
				pro.Children.Add(itemsConditionalItemTextArea);
				//format.Containers.Add(itemsConditionalItemTextArea);
			}

			{
				ItemFormatContainer itemsHorizontalCheckbox = new ItemFormatContainer();
				itemsHorizontalCheckbox.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericQuestionnaireItem" };
				itemsHorizontalCheckbox.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "LikertHorizontalCheckbox" }, ResponseType = QuestionnaireResponseType.MultiSelect });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.23", OrderInSection = 23 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.24", OrderInSection = 24 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.25", OrderInSection = 25 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.26", OrderInSection = 26 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.27", OrderInSection = 27 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.28", OrderInSection = 28 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.29", OrderInSection = 29 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.30", OrderInSection = 30 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.31", OrderInSection = 31 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.32", OrderInSection = 32 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.33", OrderInSection = 33 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.34", OrderInSection = 34 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.35", OrderInSection = 35 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.66", OrderInSection = 66 });
				pro.Children.Add(itemsHorizontalCheckbox);
				//format.Containers.Add(itemsHorizontalCheckbox);
			}

			{
				ItemFormatContainer itemsTextBoxDatePicker = new ItemFormatContainer();
				itemsTextBoxDatePicker.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericQuestionnaireItem" };
				itemsTextBoxDatePicker.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "TextBox" }, ResponseType = QuestionnaireResponseType.Text });
				itemsTextBoxDatePicker.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "DatePicker" }, ResponseType = QuestionnaireResponseType.List });
				itemsTextBoxDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.16", OrderInSection = 16 });
				itemsTextBoxDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.17", OrderInSection = 17 });
				itemsTextBoxDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.21", OrderInSection = 21 });
				itemsTextBoxDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.22", OrderInSection = 22 });
				itemsTextBoxDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.39", OrderInSection = 39 });
				itemsTextBoxDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.41", OrderInSection = 41 });
				itemsTextBoxDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.47", OrderInSection = 47 });
				itemsTextBoxDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.61", OrderInSection = 61 });
				itemsTextBoxDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.63", OrderInSection = 63 });
				itemsTextBoxDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.65", OrderInSection = 65 });
				pro.Children.Add(itemsTextBoxDatePicker);
				//format.Containers.Add(itemsTextBoxDatePicker);
			}

			{
				ItemFormatContainer itemsLikertSlider = new ItemFormatContainer();
				itemsLikertSlider.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericQuestionnaireItem" };
				itemsLikertSlider.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "LikertSlider" }, ResponseType = QuestionnaireResponseType.Range });
				itemsLikertSlider.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.55", OrderInSection = 55 });
				pro.Children.Add(itemsLikertSlider);
				//format.Containers.Add(itemsLikertSlider);
			}

			this.questionnaireFormatClient.AddOrUpdateFullFormat(format);
		}

		public void AddFirstAppointmentChatFormat()
		{
			Format format = new Format();
			format.Name = "OPSMCFirstAppointmentFormat";
			format.SupportedPlatform = Platform.Chat;

			FormatContainer pro = new FormatContainer();
			format.Containers.Add(pro);
			pro.ContainerFormatDefinition = new ContainerFormatDefinition() { ContainerDefinitionName = "GenericQuestionnaireChatCanvas" };


			{
				ItemFormatContainer itemsConditionalItemDatePicker = new ItemFormatContainer();
				//container.ContainerFormatDefinition = ""
				itemsConditionalItemDatePicker.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericChatItemDefinition" };
				itemsConditionalItemDatePicker.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatTextBox" }, ResponseType = QuestionnaireResponseType.Text });
				itemsConditionalItemDatePicker.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatDatePicker" }, ResponseType = QuestionnaireResponseType.List });
				itemsConditionalItemDatePicker.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatConditionalItem" }, ResponseType = QuestionnaireResponseType.ConditionalItem });
				itemsConditionalItemDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.15", OrderInSection = 15 });
				itemsConditionalItemDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.15.1", OrderInSection = 15 });
				itemsConditionalItemDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.37", OrderInSection = 37 });
				itemsConditionalItemDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.58", OrderInSection = 58 });
				pro.Children.Add(itemsConditionalItemDatePicker);
				//format.Containers.Add(itemsConditionalItemDatePicker);
			}

			{
				ItemFormatContainer itemsHorizontalRadio = new ItemFormatContainer();
				itemsHorizontalRadio.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericChatItemDefinition" };
				itemsHorizontalRadio.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatLikertHorizontalRadio" }, ResponseType = QuestionnaireResponseType.List });
				//itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.1", OrderInSection = 1 });
				//itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.7", OrderInSection = 7 });
				//itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.9", OrderInSection = 9 });
				//itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.10", OrderInSection = 10 });
				//itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.12", OrderInSection = 12 });
				//itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.13", OrderInSection = 13 });
				//itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.14", OrderInSection = 14 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.20", OrderInSection = 20 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.40", OrderInSection = 40 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.42", OrderInSection = 42 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.43", OrderInSection = 43 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.44", OrderInSection = 44 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.45", OrderInSection = 45 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.46", OrderInSection = 46 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.48", OrderInSection = 48 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.49", OrderInSection = 49 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.60", OrderInSection = 60 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.62", OrderInSection = 62 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.64", OrderInSection = 64 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.69", OrderInSection = 69 });
				itemsHorizontalRadio.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.70", OrderInSection = 1 });
				pro.Children.Add(itemsHorizontalRadio);
				//format.Containers.Add(itemsHorizontalRadio);
			}

			{
				ItemFormatContainer itemsBodyControl = new ItemFormatContainer();
				//container.ContainerFormatDefinition = ""
				itemsBodyControl.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "ChatBodyControlQuestionnaireItem" };
				itemsBodyControl.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatBodyControl" }, ResponseType = QuestionnaireResponseType.MultiSelect });
				itemsBodyControl.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.54", OrderInSection = 54 });
				//format.Containers.Add(itemsBodyControl);
				pro.Children.Add(itemsBodyControl);
			}

			/*{
				ItemFormatContainer itemsPassword = new ItemFormatContainer();
				itemsPassword.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericChatItemDefinition" };
				itemsPassword.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatPasswordTextBox" }, ResponseType = QuestionnaireResponseType.Text });
				itemsPassword.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.2", OrderInSection = 2 });
				itemsPassword.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.3", OrderInSection = 3 });
				itemsPassword.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.4", OrderInSection = 4 });
				itemsPassword.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.5", OrderInSection = 5 });
				itemsPassword.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.6", OrderInSection = 6 });
				pro.Children.Add(itemsPassword);
				//format.Containers.Add(itemsPassword);
			}*/

			{
				ItemFormatContainer itemsDropdown = new ItemFormatContainer();
				itemsDropdown.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericChatItemDefinition" };
				itemsDropdown.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatDropdown" }, ResponseType = QuestionnaireResponseType.List });
				itemsDropdown.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatTextBox" }, ResponseType = QuestionnaireResponseType.Text });
				itemsDropdown.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatConditionalItem" }, ResponseType = QuestionnaireResponseType.ConditionalItem });
				itemsDropdown.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.19", OrderInSection = 19 });
				itemsDropdown.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.59", OrderInSection = 59 });
				itemsDropdown.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.67", OrderInSection = 67 });
				pro.Children.Add(itemsDropdown);
				//format.Containers.Add(itemsDropdown);
			}

			{
				ItemFormatContainer itemsConditionalItem = new ItemFormatContainer();
				itemsConditionalItem.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericChatItemDefinition" };
				itemsConditionalItem.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatTextBox" }, ResponseType = QuestionnaireResponseType.Text });
				itemsConditionalItem.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatLikertHorizontalRadio" }, ResponseType = QuestionnaireResponseType.List });
				itemsConditionalItem.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatConditionalItem" }, ResponseType = QuestionnaireResponseType.ConditionalItem });
				itemsConditionalItem.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.18", OrderInSection = 18 });
				itemsConditionalItem.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.36", OrderInSection = 36 });
				itemsConditionalItem.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.38", OrderInSection = 38 });
				itemsConditionalItem.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.50", OrderInSection = 50 });
				itemsConditionalItem.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.51", OrderInSection = 51 });
				itemsConditionalItem.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.52", OrderInSection = 52 });
				itemsConditionalItem.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.53", OrderInSection = 53 });
				itemsConditionalItem.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.56", OrderInSection = 56 });
				itemsConditionalItem.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.68", OrderInSection = 68 });
				pro.Children.Add(itemsConditionalItem);
				//format.Containers.Add(itemsConditionalItem);
			}

			{
				ItemFormatContainer itemsConditionalItemTextArea = new ItemFormatContainer();
				itemsConditionalItemTextArea.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericChatItemDefinition" };
				itemsConditionalItemTextArea.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatTextArea" }, ResponseType = QuestionnaireResponseType.Text });
				itemsConditionalItemTextArea.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatLikertHorizontalRadio" }, ResponseType = QuestionnaireResponseType.List });
				itemsConditionalItemTextArea.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatConditionalItem" }, ResponseType = QuestionnaireResponseType.ConditionalItem });
				itemsConditionalItemTextArea.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.57", OrderInSection = 57 });
				pro.Children.Add(itemsConditionalItemTextArea);
				//format.Containers.Add(itemsConditionalItemTextArea);
			}

			{
				ItemFormatContainer itemsHorizontalCheckbox = new ItemFormatContainer();
				itemsHorizontalCheckbox.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericChatItemDefinition" };
				itemsHorizontalCheckbox.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatLikertHorizontalCheckbox" }, ResponseType = QuestionnaireResponseType.MultiSelect });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.23", OrderInSection = 23 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.24", OrderInSection = 24 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.25", OrderInSection = 25 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.26", OrderInSection = 26 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.27", OrderInSection = 27 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.28", OrderInSection = 28 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.29", OrderInSection = 29 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.30", OrderInSection = 30 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.31", OrderInSection = 31 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.32", OrderInSection = 32 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.33", OrderInSection = 33 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.34", OrderInSection = 34 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.35", OrderInSection = 35 });
				itemsHorizontalCheckbox.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.66", OrderInSection = 66 });
				pro.Children.Add(itemsHorizontalCheckbox);
				//format.Containers.Add(itemsHorizontalCheckbox);
			}

			{
				ItemFormatContainer itemsTextBoxDatePicker = new ItemFormatContainer();
				itemsTextBoxDatePicker.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericChatItemDefinition" };
				itemsTextBoxDatePicker.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatTextBox" }, ResponseType = QuestionnaireResponseType.Text });
				itemsTextBoxDatePicker.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatDatePicker" }, ResponseType = QuestionnaireResponseType.List });
				//itemsTextBoxDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.8", OrderInSection = 8 });
				//itemsTextBoxDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.11", OrderInSection = 11 });
				itemsTextBoxDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.16", OrderInSection = 16 });
				itemsTextBoxDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.17", OrderInSection = 17 });
				itemsTextBoxDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.21", OrderInSection = 21 });
				itemsTextBoxDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.22", OrderInSection = 22 });
				itemsTextBoxDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.39", OrderInSection = 39 });
				itemsTextBoxDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.41", OrderInSection = 41 });
				itemsTextBoxDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.47", OrderInSection = 47 });
				itemsTextBoxDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.61", OrderInSection = 61 });
				itemsTextBoxDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.63", OrderInSection = 63 });
				itemsTextBoxDatePicker.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.65", OrderInSection = 65 });
				pro.Children.Add(itemsTextBoxDatePicker);
				//format.Containers.Add(itemsTextBoxDatePicker);
			}

			{
				ItemFormatContainer itemsLikertSlider = new ItemFormatContainer();
				itemsLikertSlider.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericChatItemDefinition" };
				itemsLikertSlider.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatLikertSlider" }, ResponseType = QuestionnaireResponseType.Range });
				itemsLikertSlider.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.55", OrderInSection = 55 });
				pro.Children.Add(itemsLikertSlider);
				//format.Containers.Add(itemsLikertSlider);
			}
			/*format.Containers.Add(itemsTextBoxDatePicker);
			format.Containers.Add(itemsHorizontalRadio);
			format.Containers.Add(itemsLikertSlider);
			
			format.Containers.Add(itemsHorizontalCheckbox);
			format.Containers.Add(itemsConditionalItem);*/

			this.questionnaireFormatClient.AddOrUpdateFullFormat(format);
		}

		public void AddCurrentConditionClassicFormat()
		{
			Format format = new Format();
			format.Name = "OPSMCCurrentConditionFormat";
			format.SupportedPlatform = Platform.Classic;

			FormatContainer pro = new FormatContainer();
			format.Containers.Add(pro);
			pro.ContainerFormatDefinition = new ContainerFormatDefinition() { ContainerDefinitionName = "GenericQuestionnaireCanvas" };

			{
				ItemFormatContainer itemsBodyControl = new ItemFormatContainer();
				//container.ContainerFormatDefinition = ""
				itemsBodyControl.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "BodyControlQuestionnaireItem" };
				itemsBodyControl.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "BodyControl" }, ResponseType = QuestionnaireResponseType.MultiSelect });
				itemsBodyControl.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.CC.1", OrderInSection = 1 });
				//format.Containers.Add(itemsBodyControl);
				pro.Children.Add(itemsBodyControl);
			}

			{
				ItemFormatContainer itemsLikertSlider = new ItemFormatContainer();
				itemsLikertSlider.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericQuestionnaireItem" };
				itemsLikertSlider.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "LikertSlider" }, ResponseType = QuestionnaireResponseType.Range });
				itemsLikertSlider.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.CC.2", OrderInSection = 2 });
				pro.Children.Add(itemsLikertSlider);
				//format.Containers.Add(itemsLikertSlider);
			}

			{
				ItemFormatContainer itemsConditionalItem = new ItemFormatContainer();
				itemsConditionalItem.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericQuestionnaireItem" };
				itemsConditionalItem.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "TextBox" }, ResponseType = QuestionnaireResponseType.Text });
				itemsConditionalItem.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "LikertHorizontalRadio" }, ResponseType = QuestionnaireResponseType.List });
				itemsConditionalItem.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ConditionalItem" }, ResponseType = QuestionnaireResponseType.ConditionalItem });
				itemsConditionalItem.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.CC.3", OrderInSection = 3 });
				pro.Children.Add(itemsConditionalItem);
				//format.Containers.Add(itemsConditionalItem);
			}

			{
				ItemFormatContainer itemsConditionalItemTextArea = new ItemFormatContainer();
				itemsConditionalItemTextArea.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericQuestionnaireItem" };
				itemsConditionalItemTextArea.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "TextArea" }, ResponseType = QuestionnaireResponseType.Text });
				itemsConditionalItemTextArea.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "LikertHorizontalRadio" }, ResponseType = QuestionnaireResponseType.List });
				itemsConditionalItemTextArea.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ConditionalItem" }, ResponseType = QuestionnaireResponseType.ConditionalItem });
				itemsConditionalItemTextArea.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.CC.4", OrderInSection = 4 });
				pro.Children.Add(itemsConditionalItemTextArea);
				//format.Containers.Add(itemsConditionalItemTextArea);
			}

			this.questionnaireFormatClient.AddOrUpdateFullFormat(format);

		}

		public void AddCurrentConditionChatFormat()
		{
			Format format = new Format();
			format.Name = "OPSMCCurrentConditionFormat";
			format.SupportedPlatform = Platform.Chat;

			FormatContainer pro = new FormatContainer();
			format.Containers.Add(pro);
			pro.ContainerFormatDefinition = new ContainerFormatDefinition() { ContainerDefinitionName = "GenericQuestionnaireChatCanvas" };

			{
				ItemFormatContainer itemsBodyControl = new ItemFormatContainer();
				//container.ContainerFormatDefinition = ""
				itemsBodyControl.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "ChatBodyControlQuestionnaireItem" };
				itemsBodyControl.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatBodyControl" }, ResponseType = QuestionnaireResponseType.MultiSelect });
				itemsBodyControl.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.CC.1", OrderInSection = 1 });
				//format.Containers.Add(itemsBodyControl);
				pro.Children.Add(itemsBodyControl);
			}

			{
				ItemFormatContainer itemsLikertSlider = new ItemFormatContainer();
				itemsLikertSlider.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericChatItemDefinition" };
				itemsLikertSlider.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatLikertSlider" }, ResponseType = QuestionnaireResponseType.Range });
				itemsLikertSlider.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.CC.2", OrderInSection = 2 });
				pro.Children.Add(itemsLikertSlider);
				//format.Containers.Add(itemsLikertSlider);
			}

			{
				ItemFormatContainer itemsConditionalItem = new ItemFormatContainer();
				itemsConditionalItem.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericChatItemDefinition" };
				itemsConditionalItem.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatTextBox" }, ResponseType = QuestionnaireResponseType.Text });
				itemsConditionalItem.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatLikertHorizontalRadio" }, ResponseType = QuestionnaireResponseType.List });
				itemsConditionalItem.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatConditionalItem" }, ResponseType = QuestionnaireResponseType.ConditionalItem });
				itemsConditionalItem.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.CC.3", OrderInSection = 3 });
				pro.Children.Add(itemsConditionalItem);
				//format.Containers.Add(itemsConditionalItem);
			}

			{
				ItemFormatContainer itemsConditionalItemTextArea = new ItemFormatContainer();
				itemsConditionalItemTextArea.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericChatItemDefinition" };
				itemsConditionalItemTextArea.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatTextArea" }, ResponseType = QuestionnaireResponseType.Text });
				itemsConditionalItemTextArea.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatLikertHorizontalRadio" }, ResponseType = QuestionnaireResponseType.List });
				itemsConditionalItemTextArea.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatConditionalItem" }, ResponseType = QuestionnaireResponseType.ConditionalItem });
				itemsConditionalItemTextArea.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OPSMC.CC.4", OrderInSection = 4 });
				pro.Children.Add(itemsConditionalItemTextArea);
				//format.Containers.Add(itemsConditionalItemTextArea);
			}

			this.questionnaireFormatClient.AddOrUpdateFullFormat(format);

		}

		public void AddQuestionnaireFormat()
		{
			Format format = new Format();
			format.Name = "OESStandardFormat";
			format.SupportedPlatform = Platform.Classic;
			FormatContainer pro = new FormatContainer();
			format.Containers.Add(pro);
			pro.ContainerFormatDefinition = new ContainerFormatDefinition() { ContainerDefinitionName = "GenericQuestionnaireCanvas" };
			{
				TextFormatContainer intro = new TextFormatContainer();
				//container.ContainerFormatDefinition = new ContainerFormatDefinition() { ContainerDefinitionName = "Defines start and end of Table" };                
				intro.TextFormatDefinition = new TextFormatDefinition() { ElementFormatDefinitionName = "GenericQuestionnaireText" };
				intro.Elements.Add(new FormatContainerElement() { OrderInSection = 1, QuestionnaireElementActionId = "Intro1" });
				intro.Elements.Add(new FormatContainerElement() { OrderInSection = 2, QuestionnaireElementActionId = "Intro2" });
				pro.Children.Add(intro);
			}

			{
				ItemFormatContainer items = new ItemFormatContainer();
				//container.ContainerFormatDefinition = ""
				items.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericQuestionnaireItem" };
				items.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "LikertHorizontalRadio" }, ResponseType = QuestionnaireResponseType.List });
				items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.1", OrderInSection = 1 });
				items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.2", OrderInSection = 2 });
				//items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.3", OrderInSection = 3 });
				//items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.4", OrderInSection = 4 });
				pro.Children.Add(items);
			}

			{
				ItemFormatContainer items = new ItemFormatContainer();
				//container.ContainerFormatDefinition = ""
				items.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericQuestionnaireItem" };
				items.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "LikertHorizontalRadio" }, ResponseType = QuestionnaireResponseType.List });
				items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.3", OrderInSection = 3 });
				items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.4", OrderInSection = 4 });
				pro.Children.Add(items);
			}

			{
				ItemFormatContainer items = new ItemFormatContainer();
				items.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "BodyControlQuestionnaireItem" };
				items.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "BodyControl" }, ResponseType = QuestionnaireResponseType.MultiSelect });
				items.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "OES.13", OrderInSection = 5 });
				pro.Children.Add(items);
			}

			{
				TextFormatContainer footer = new TextFormatContainer();
				//container.ContainerFormatDefinition = new ContainerFormatDefinition() { ContainerDefinitionName = "Defines start and end of Table" };                
				footer.TextFormatDefinition = new TextFormatDefinition() { ElementFormatDefinitionName = "GenericQuestionnaireText" };
				footer.Elements.Add(new FormatContainerElement() { OrderInSection = 1, QuestionnaireElementActionId = "Footer1" });
				pro.Children.Add(footer);
			}

			this.questionnaireFormatClient.AddOrUpdateFullFormat(format);
		}
		public void AddTagToQuestionnaireByName()
		{
			this.questionnaireClient.AddTagToQuestionnaireByName("age", "young", "OES2");
		}

		public void AddPatient()
		{
			OperationResult result = this.patientClient.CreatePatient("123", "erick.dario2492@gmail.com", "erick.dario2492@gmail.com", "Mr.", "Erick", "Garcia", DateTime.Now, "1234567890");
		}

		public static void Main(string[] args)
		{
			Program p = new Program();

			//p.RetrieveCompleteProInstrumentTest();
			//p.AddUser();
			//p.SaveQuestionnaireResponseTest();
			//=======================================
			
			//p.AddPatientRegistration();
			
			p.AddFullProInstrumentTest();
			p.AddQuestionnaireFormatDefinition();
			p.AddQuestionnaireFormat();
			p.AddChatFormatDefinition();
			p.AddChatFormat();

			p.FirstAppointmentQuestionnaire();
			p.AddFirstAppointmentClassicFormat();
			p.AddFirstAppointmentChatFormat();

			p.CurrentConditionQuestionnaire();
			p.AddCurrentConditionClassicFormat();
			p.AddCurrentConditionChatFormat();
			
			/*
			p.AddPatientRegistration();
			p.AddPatientRegistrationFormat();*/

			//=======================================




			//p.AddFullProInstrumentTest();
			/*
			p.AddTagToQuestionnaireByName();
			p.RetrieveCompleteProInstrumentTest();
			//p.SaveQuestionnaireResponseTest();
			ProInstrument pro = p.CreateInstrument();
			XmlObjectSerializer serializer = new DataContractSerializer(pro.GetType(), "ProInstrument", "", null,
				0x7FFF, // maxItemsInObjectGraph
				false,  // ignoreExtensionDataObject
				true,   // preserveObjectReferences
				null);  // dataContractSurrogate
			
			StringWriter sww = new StringWriter();
			XmlWriter writer = XmlWriter.Create(sww);
			serializer.WriteStartObject(writer, pro);
			serializer.WriteObjectContent(writer, pro);
			serializer.WriteEndObject(writer);
			writer.Flush();            
			string xml = sww.ToString(); // Your xml


			TextReader tr = new StringReader(xml);
			XmlReader reader = XmlReader.Create(tr);
			var proOUt = serializer.ReadObject(reader);
			 */
		}

		private void AddPatientRegistration()
		{
			Survey pro = new Survey();
			pro.Name = "Registration";
			pro.DisplayName = "Registration";
			pro.Status = QuestionnaireStatus.Indevelopment;
			pro.IsActive = true;
			pro.DefaultFormatName = "RegistrationFormat";

			pro.Concept = new QuestionnaireConcept() { Name = "Registration", Description = "Complete Registration Questionnaire" };

			QuestionnaireSection section = new QuestionnaireSection();
			pro.Sections.Add(section);

			QuestionnaireItem qi = AddItemToSection(section, "REG.1", "", @"Hello <%PatientFirstName%/>! Welcome to RePLAY, a groundbreaking new service that allows you to track your health progress and prepare for your upcoming appointments at OPSMC.  Here at OPSMC we take your privacy very seriously. We’ve put a lot of time and effort into keeping your information secure and you can see just how we do this in our strict privacy and confidentiality policy. Next we’re going to choose a good password.", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
			AddOptionGroupToItem(qi, new MyDictionary(), 0, QuestionnaireResponseType.List, optionGroupText: "Okay!");

			//qi = AddItemToSection(section, "REG.2", "", @"", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
			qi = AddItemToSection(section, "REG.2", "The best way to keep your information as secure as Fort Knox is to choose a strong password. This means your password will need to have at least: 8 characters, a capital letter, a number and, a symbol. Please type in a password that fits these criteria, and make sure it’s one that you’ll remember!", @"", Platform.Classic | Platform.Chat, Instance.Baseline | Instance.Followup);
			AddOptionGroupToItem(qi, new MyDictionary(), 0, QuestionnaireResponseType.Text, "<table><tr><td>Password:</td><td><input name=\"Password\" type=\"password\" /></td></tr><tr><td>Repeat password</td><td><input name=\"passwordrepeat\" type=\"password\" /></td></tr></table>", null);


			var result = this.questionnaireClient.SaveFullQuestionnaire(pro);
		}

		private void AddPatientRegistrationFormat()
		{
			Format format = new Format();
			format.Name = "RegistrationFormat";
			format.SupportedPlatform = Platform.Chat;


			FormatContainer pro = new FormatContainer();
			format.Containers.Add(pro);
			pro.ContainerFormatDefinition = new ContainerFormatDefinition() { ContainerDefinitionName = "GenericQuestionnaireChatCanvas" };


			ItemFormatContainer itemsConditionalItem = new ItemFormatContainer();
			itemsConditionalItem.ItemFormatDefinition = new ItemFormatDefinition() { ElementFormatDefinitionName = "GenericChatItemDefinition" };
			itemsConditionalItem.ItemGroupFormats.Add(new ItemGroupFormat() { ItemGroupOptionsFormatDefinition = new ItemGroupOptionsFormatDefinition() { GroupOptionDefinitionName = "ChatTextBox" }, ResponseType = QuestionnaireResponseType.Text });
			itemsConditionalItem.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "REG.1", OrderInSection = 1 });
			itemsConditionalItem.Elements.Add(new FormatContainerElement() { QuestionnaireElementActionId = "REG.2", OrderInSection = 1 });
			//pro.Children.Add(itemsConditionalItem);
			format.Containers.Add(itemsConditionalItem);
		}
	}
}
