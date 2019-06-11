<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="creadorxml.aspx.cs" Inherits="crearxml.creadorxml" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        #pregunta1 {
            margin-left: 6px;
        }
        #Respuesta12 {
            margin-left: 6px;
        }
        #Respuesta11 {
            margin-left: 9px;
        }
        #Respuesta13 {
            margin-left: 7px;
        }
        #Respuesta14 {
            margin-left: 5px;
        }
        #Text4 {
            margin-left: 2px;
        }
        #Answer4 {
            margin-left: 2px;
        }
        #nombreArchivo {
            margin-left: 23px;
        }
        #Text1 {
            margin-left: 15px;
        }
        #Text2 {
            margin-left: 73px;
        }
        #Text3 {
            margin-left: 77px;
        }
        #Text5 {
            margin-left: 1px;
        }
        .auto-style1 {
            font-size: x-large;
        }
        .auto-style3 {
            height: 38px;
            font-size: xx-large;
            font-weight: normal;
        }
        .auto-style4 {
            color: #6699FF;
            background-color: #FFFFFF;
        }
        .auto-style5 {
            color: #6699FF;
        }
        .auto-style6 {
            color: #3333CC;
            background-color: #FFFFFF;
        }
    </style>
</head>
<body>
    <form id="form1" method="post"  runat="server">
    <div>
   <h3 class="auto-style3"><strong><span class="auto-style6">OES/ Oxford Elbow Score</span></strong></h3>
        <p class="auto-style1"><strong><span class="auto-style6">Questionnaire OES</span></strong></p>
        <p><span class="auto-style4">Questionnaire Name:</span><input type="text" id="nombreArchivo" name="nombreArchivo" runat="server" class="auto-style5"/></p>
        <p><span class="auto-style4">First Name of Patient:</span><input type="text" id="Text1" name="nombre" runat="server" class="auto-style5"/></p>
        <p><span class="auto-style4">Patient Age:</span><input type="text" id="Text2" name="edad" runat="server" class="auto-style5"/></p>
        <p style="background-color: #FFFFFF"><span class="auto-style4">Last Name:</span><input type="text" id="Text3" name="apellido" runat="server" class="auto-style5"/></p>
        <p>
            <asp:Label ID="Label1" runat="server" Height="200px" Text="Salida:" Width="500px"></asp:Label>
        </p>
        <p></p>
        <fieldset>
        <p><span class="auto-style5">Question1</span><input type="text" id="pregunta1" name="pregunta1" runat="server" class="auto-style5"/><span class="auto-style5"> ¿Have you had difficulty lifting things in your home, suchs as putting out the rubbish?</span></p>
        <p class="auto-style5">************************************************************</p>
        <p><span class="auto-style5">Answer0</span><input type="text" id="Respuesta11" name="Respuesta11" runat="server" class="auto-style5"/><span class="auto-style5"> No dificult.</span></p>
        <p><span class="auto-style5">Answer1</span><input type="text" id="Respuesta12" name="Respuesta12" runat="server" class="auto-style5"/><span class="auto-style5"> A Little bit of difficulty.</span></p>
        <p><span class="auto-style5">Answer2</span><input type="text" id="Respuesta13" name="Respuesta13" runat="server" class="auto-style5"/><span class="auto-style5">Moderate difficult.</span></p>
        <p><span class="auto-style5">Answer3</span><input type="text" id="Respuesta14" name="Respuesta14" runat="server" class="auto-style5"/><span class="auto-style5">Extreme difficult.</span></p>
        <p><span class="auto-style5">Answer4</span><input type="text" id="Respuesta15" name="Respuesta15" runat="server" class="auto-style5" /><span class="auto-style5">Impossible to do.</span></p>
                </p>
        </fieldset>
        <p></p>
        <fieldset>
        <p><span class="auto-style5">Question2</span><input type="text" id="pregunta2" name="pregunta2" runat="server" class="auto-style5"/><span class="auto-style5">¿Have you had difficulty carrying bags of shopping?</span></p>
         <p class="auto-style5">************************************************************</p>
        <p><span class="auto-style5">Answer0</span><input type="text" id="Respuesta21" name="Respuesta21" runat="server" class="auto-style5"/><span class="auto-style5"> No difficult.</span></p>
        <p><span class="auto-style5">Answer1</span><input type="text" id="Respuesta22" name="Respuesta22" runat="server" class="auto-style5"/><span class="auto-style5"> A Little bit of difficulty.</span></p>
        <p><span class="auto-style5">Answer2</span><input type="text" id="Respuesta23" name="Respuesta23" runat="server" class="auto-style5"/><span class="auto-style5"> Moderate difficult.</span></p>
        <p><span class="auto-style5">Answer3</span><input type="text" id="Respuesta24" name="Respuesta24" runat="server" class="auto-style5"/><span class="auto-style5">Extreme difficult.</span></p>
        <p><span class="auto-style5">Answer4</span><input type="text" id="Respuesta25" name="Respuesta25" runat="server" class="auto-style5" /><span class="auto-style5">Impossible to do.</span><p class="auto-style5">
                &nbsp;</p>
        </fieldset><p></p><fieldset>
        <p><span class="auto-style5">Question3</span><input type="text" id="pregunta3" name="pregunta3" runat="server" class="auto-style5"/><span class="auto-style5">¿ Have you had any difficulty washing yourself?</span></p>
         <p class="auto-style5">************************************************************</p>
        <p><span class="auto-style5">Answer0</span><input type="text" id="Respuesta31" name="Respuesta31" runat="server" class="auto-style5"/><span class="auto-style5"> No difficult.</span></p>
        <p><span class="auto-style5">Answer1</span><input type="text" id="Respuesta32" name="Respuesta32" runat="server" class="auto-style5"/><span class="auto-style5"> A Little bit of difficulty.</span></p>
        <p><span class="auto-style5">Answer2</span><input type="text" id="Respuesta33" name="Respuesta33" runat="server" class="auto-style5"/><span class="auto-style5"> Moderate difficult.</span></p>
        <p><span class="auto-style5">Answer3</span><input type="text" id="Respuesta34" name="Respuesta34" runat="server" class="auto-style5"/><span class="auto-style5">Extreme difficult.</span></p>
        <p><span class="auto-style5">Answer4</span><input type="text" id="Respuesta35" name="Respuesta35" runat="server" class="auto-style5"/><span class="auto-style5">Impossible to do.</span></p>
        </fieldset><p></p><fieldset>
        <p><span class="auto-style5">Question4</span><input type="text" id="pregunta4" name="pregunta4" runat="server" class="auto-style5"/><span class="auto-style5">¿Have you had any difficulty dressing youself?</span></p>
        <p class="auto-style5">************************************************************</p>
        <p><span class="auto-style5">Answer0</span><input type="text" id="Respuesta41" name="Respuesta41" runat="server" class="auto-style5"/><span class="auto-style5"> No difficult.</span></p>
        <p><span class="auto-style5">Answer1</span><input type="text" id="Respuesta42" name="Respuesta42" runat="server" class="auto-style5"/><span class="auto-style5"> A Little bit of difficulty.</span></p>
        <p><span class="auto-style5">Answer2</span><input type="text" id="Respuesta43" name="Respuesta43" runat="server" class="auto-style5"/><span class="auto-style5"> Moderate difficult.</span></p>
        <p><span class="auto-style5">Answer3</span><input type="text" id="Respuesta44" name="Respuesta44" runat="server" class="auto-style5"/><span class="auto-style5">Extreme difficult.</span></p>
        <p><span class="auto-style5">Answer4</span><input type="text" id="Respuesta45" name="Respuesta45" runat="server" class="auto-style5"/><span class="auto-style5">Impossible to do.</span></p>
        </fieldset>
        <p></p>
        <fieldset>
        <p><span class="auto-style5">Question5</span><input type="text" id="pregunta5" name="pregunta1" runat="server" class="auto-style5"/><span class="auto-style5">¿Have you felt that you elbow problem is&quot;controlling your life&quot;?</span></p>
        <p class="auto-style5">************************************************************</p>
        <p><span class="auto-style5">Answer0</span><input type="text" id="Respuesta51" name="Respuesta51" runat="server" class="auto-style5"/><span class="auto-style5"> No difficult.</span></p>
        <p><span class="auto-style5">Answer1</span><input type="text" id="Respuesta52" name="Respuesta52" runat="server" class="auto-style5"/><span class="auto-style5"> A Little bit of difficulty.</span></p>
        <p><span class="auto-style5">Answer2</span><input type="text" id="Respuesta53" name="Respuesta53" runat="server" class="auto-style5"/><span class="auto-style5"> Moderate difficult.</span></p>
        <p><span class="auto-style5">Answer3</span><input type="text" id="Respuesta54" name="Respuesta54" runat="server" class="auto-style5"/><span class="auto-style5">Extreme difficult.</span></p>
        <p><span class="auto-style5">Answer4</span><input type="text" id="Respuesta55" name="Respuesta55" runat="server" class="auto-style5" /><span class="auto-style5">Impossible to do.</span></p>
                <p> 
        </fieldset>
        <p></p>
        <fieldset>
        <p><span class="auto-style5">Question6</span><input type="text" id="pregunta6" name="pregunta1" runat="server" class="auto-style5"/><span class="auto-style5">¿How much has your elbow problem been&quot;On your mind&quot;?</span></p>
        <p class="auto-style5">************************************************************</p>
        <p><span class="auto-style5">Answer0</span><input type="text" id="Respuesta61" name="Respuesta61" runat="server" class="auto-style5"/><span class="auto-style5"> No difficult.</span></p>
        <p><span class="auto-style5">Answer1</span><input type="text" id="Respuesta62" name="Respuesta62" runat="server" class="auto-style5"/><span class="auto-style5"> A Little bit of difficulty.</span></p>
        <p><span class="auto-style5">Answer2</span><input type="text" id="Respuesta63" name="Respuesta63" runat="server" class="auto-style5"/><span class="auto-style5"> Moderate difficult.</span></p>
        <p><span class="auto-style5">Answer3</span><input type="text" id="Respuesta64" name="Respuesta64" runat="server" class="auto-style5"/><span class="auto-style5">Extreme difficult.</span></p>
        <p><span class="auto-style5">Answer4</span><input type="text" id="Respuesta65" name="Respuesta65" runat="server" class="auto-style5" /><span class="auto-style5">Impossible to do.</span></p>
                </p>
        </fieldset>
        <p></p>
        <fieldset>
        <p><span class="auto-style5">Question7</span><input type="text" id="pregunta7" name="pregunta1" runat="server" class="auto-style5"/><span class="auto-style5">¿ Have you been trouble by paln from you elbow in bed at nigth?</span></p>
        <p class="auto-style5">************************************************************</p>
        <p><span class="auto-style5">Answer0</span><input type="text" id="Respuesta71" name="Respuesta71" runat="server" class="auto-style5"/><span class="auto-style5"> No difficult.</span></p>
        <p><span class="auto-style5">Answer1</span><input type="text" id="Respuesta72" name="Respuesta72" runat="server" class="auto-style5"/><span class="auto-style5"> A Little bit of difficulty.</span></p>
        <p><span class="auto-style5">Answer2</span><input type="text" id="Respuesta73" name="Respuesta73" runat="server" class="auto-style5"/><span class="auto-style5"> Moderate difficult.</span></p>
        <p><span class="auto-style5">Answer3</span><input type="text" id="Respuesta74" name="Respuesta74" runat="server" class="auto-style5"/><span class="auto-style5">Extreme difficult.</span></p>
        <p><span class="auto-style5">Answer4</span><input type="text" id="Respuesta75" name="Respuesta75" runat="server" class="auto-style5" /><span class="auto-style5">Impossible to do.</span></p>
                </p>
        </fieldset>
        <p></p>
        <fieldset>
        <p><span class="auto-style5">Question8</span><input type="text" id="pregunta8" name="pregunta1" runat="server" class="auto-style5"/><span class="auto-style5">¿How often has you elbow pain interfered with your sleeping?</span></p>
        <p class="auto-style5">************************************************************</p>
        <p><span class="auto-style5">Answer0</span><input type="text" id="Respuesta81" name="Respuesta81" runat="server" class="auto-style5"/><span class="auto-style5"> No difficult.</span></p>
        <p><span class="auto-style5">Answer1</span><input type="text" id="Respuesta82" name="Respuesta82" runat="server" class="auto-style5"/><span class="auto-style5"> A Little bit of difficulty.</span></p>
        <p><span class="auto-style5">Answer2</span><input type="text" id="Respuesta83" name="Respuesta83" runat="server" class="auto-style5"/><span class="auto-style5"> Moderate difficult.</span></p>
        <p><span class="auto-style5">Answer3</span><input type="text" id="Respuesta84" name="Respuesta84" runat="server" class="auto-style5"/><span class="auto-style5">Extreme difficult.</span></p>
        <p><span class="auto-style5">Answer4</span><input type="text" id="Respuesta85" name="Respuesta85" runat="server" class="auto-style5" /><span class="auto-style5">Impossible to do.</span></p>
                </p>
        </fieldset>
        <p></p>
        <fieldset>
        <p><span class="auto-style5">Question9</span><input type="text" id="pregunta9" name="pregunta1" runat="server" class="auto-style5"/><span class="auto-style5">¿ How much has you elbow problem interfered with your usual work or everyday activities?</span></p>
        <p class="auto-style5">************************************************************</p>
        <p><span class="auto-style5">Answer0</span><input type="text" id="Respuesta91" name="Respuesta91" runat="server" class="auto-style5"/><span class="auto-style5"> No difficult.</span></p>
        <p><span class="auto-style5">Answer1</span><input type="text" id="Respuesta92" name="Respuesta92" runat="server" class="auto-style5"/><span class="auto-style5"> A Little bit of difficulty.</span></p>
        <p><span class="auto-style5">Answer2</span><input type="text" id="Respuesta93" name="Respuesta93" runat="server" class="auto-style5"/><span class="auto-style5"> Moderate difficult.</span></p>
        <p><span class="auto-style5">Answer3</span><input type="text" id="Respuesta94" name="Respuesta94" runat="server" class="auto-style5"/><span class="auto-style5">Extreme difficult.</span></p>
        <p><span class="auto-style5">Answer4</span><input type="text" id="Respuesta95" name="Respuesta95" runat="server" class="auto-style5" /><span class="auto-style5">Impossible to do.</span></p>
                </p>
        </fieldset>
        <p></p>
        <fieldset>
        <p><span class="auto-style5">Question10</span><input type="text" id="pregunta10" name="pregunta1" runat="server" class="auto-style5"/><span class="auto-style5">¿ Has you elbow problem limited you ability to take part in leisure activities that you enjoy doing?</span></p>
        <p class="auto-style5">************************************************************</p>
        <p><span class="auto-style5">Answer0</span><input type="text" id="Respuesta101" name="Respuesta101" runat="server" class="auto-style5"/><span class="auto-style5"> No difficult.</span></p>
        <p><span class="auto-style5">Answer1</span><input type="text" id="Respuesta102" name="Respuesta102" runat="server" class="auto-style5"/><span class="auto-style5"> A Little bit of difficulty.</span></p>
        <p><span class="auto-style5">Answer2</span><input type="text" id="Respuesta103" name="Respuesta103" runat="server" class="auto-style5"/><span class="auto-style5"> Moderate difficult.</span></p>
        <p><span class="auto-style5">Answer3</span><input type="text" id="Respuesta104" name="Respuesta104" runat="server" class="auto-style5"/><span class="auto-style5">Extreme difficult.</span></p>
        <p><span class="auto-style5">Answer4</span><input type="text" id="Respuesta105" name="Respuesta105" runat="server" class="auto-style5" /><span class="auto-style5">Impossible to do.</span></p>
                </p>
        </fieldset>
        <p></p>
        <fieldset>
        <p><span class="auto-style5">Question11</span><input type="text" id="pregunta11" name="pregunta1" runat="server" class="auto-style5"/><span class="auto-style5">¿ How would you describe the worst pain you have from you elbow?</span></p>
        <p class="auto-style5">************************************************************</p>
        <p><span class="auto-style5">Answer0</span><input type="text" id="Respuesta111" name="Respuesta111" runat="server" class="auto-style5"/><span class="auto-style5"> No difficult.</span></p>
        <p><span class="auto-style5">Answer1</span><input type="text" id="Respuesta112" name="Respuesta112" runat="server" class="auto-style5"/><span class="auto-style5"> A Little bit of difficulty.</span></p>
        <p><span class="auto-style5">Answer2</span><input type="text" id="Respuesta113" name="Respuesta113" runat="server" class="auto-style5"/><span class="auto-style5"> Moderate difficult.</span></p>
        <p><span class="auto-style5">Answer3</span><input type="text" id="Respuesta114" name="Respuesta114" runat="server" class="auto-style5"/><span class="auto-style5">Extreme difficult.</span></p>
        <p><span class="auto-style5">Answer4</span><input type="text" id="Respuesta115" name="Respuesta115" runat="server" class="auto-style5" /><span class="auto-style5">Impossible to do.</span></p>
                </p>
        </fieldset>
        <p></p>
        <fieldset>
        <p><span class="auto-style5">Question12</span><input type="text" id="pregunta12" name="pregunta1" runat="server" class="auto-style5"/><span class="auto-style5">¿How wold you describe the pain you usually have from your elbow?</span></p>
        <p class="auto-style5">************************************************************</p>
        <p><span class="auto-style5">Answer0</span><input type="text" id="Respuesta121" name="Respuesta121" runat="server" class="auto-style5"/><span class="auto-style5"> No difficult.</span></p>
        <p><span class="auto-style5">Answer1</span><input type="text" id="Respuesta122" name="Respuesta122" runat="server" class="auto-style5"/><span class="auto-style5"> A Little bit of difficulty.</span></p>
        <p><span class="auto-style5">Answer2</span><input type="text" id="Respuesta123" name="Respuesta123" runat="server" class="auto-style5"/><span class="auto-style5"> Moderate difficult.</span></p>
        <p><span class="auto-style5">Answer3</span><input type="text" id="Respuesta124" name="Respuesta124" runat="server" class="auto-style5"/><span class="auto-style5">Extreme difficult.</span></p>
        <p><span class="auto-style5">Answer4</span><input type="text" id="Respuesta125" name="Respuesta125" runat="server" class="auto-style5" /><span class="auto-style5">Impossible to do.</span></p>
                </p>
        </fieldset>
        <p></p>
        <asp:Button ID="enviar" runat="server" OnClick="enviar_Click1" Text="Enviar" />
    </div>
    </form>
</body>
</html>
