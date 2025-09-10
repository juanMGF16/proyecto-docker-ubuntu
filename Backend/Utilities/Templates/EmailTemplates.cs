namespace Utilities.Templates
{
    public static class EmailTemplates
    {
        public static string GetPasswordRecoveryTemplate(string username, string recoveryLink, int expirationHours = 24)
        {
            return $@"
                <!DOCTYPE html>
                <html lang='es'>
                <head>
                    <meta charset='utf-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1'>
                    <title>Recuperación de Contraseña</title>
                    <style>
                        body {{
                            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                            line-height: 1.6;
                            color: #333;
                            margin: 0;
                            padding: 0;
                            background-color: #f4f4f4;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 20px auto;
                            background-color: #ffffff;
                            border-radius: 10px;
                            overflow: hidden;
                            box-shadow: 0 0 20px rgba(0,0,0,0.1);
                        }}
                        .header {{
                            background: linear-gradient(135deg, #007bff, #0056b3);
                            color: white;
                            padding: 30px 20px;
                            text-align: center;
                        }}
                        .header h1 {{
                            margin: 0;
                            font-size: 28px;
                            font-weight: 600;
                        }}
                        .content {{
                            padding: 40px 30px;
                            background-color: #f8f9fa;
                        }}
                        .greeting {{
                            font-size: 18px;
                            color: #2c3e50;
                            margin-bottom: 20px;
                        }}
                        .message {{
                            font-size: 16px;
                            color: #34495e;
                            margin-bottom: 25px;
                            line-height: 1.8;
                        }}
                        .button-container {{
                            text-align: center;
                            margin: 30px 0;
                        }}
                        .button {{
                            background: linear-gradient(135deg, #28a745, #20c997);
                            color: white;
                            padding: 16px 32px;
                            text-decoration: none;
                            border-radius: 50px;
                            display: inline-block;
                            font-weight: 600;
                            font-size: 16px;
                            text-transform: uppercase;
                            letter-spacing: 0.5px;
                            box-shadow: 0 4px 15px rgba(40, 167, 69, 0.3);
                            transition: all 0.3s ease;
                        }}
                        .button:hover {{
                            background: linear-gradient(135deg, #20c997, #28a745);
                            transform: translateY(-2px);
                            box-shadow: 0 6px 20px rgba(40, 167, 69, 0.4);
                        }}
                        .warning {{
                            background-color: #fff3cd;
                            border-left: 4px solid #ffc107;
                            padding: 15px;
                            margin: 20px 0;
                            border-radius: 4px;
                        }}
                        .footer {{
                            text-align: center;
                            padding: 25px;
                            background-color: #2c3e50;
                            color: #ecf0f1;
                            font-size: 12px;
                        }}
                        .footer a {{
                            color: #3498db;
                            text-decoration: none;
                        }}
                        .expiration {{
                            background-color: #e8f5e8;
                            border: 1px solid #28a745;
                            border-radius: 5px;
                            padding: 12px;
                            margin: 15px 0;
                            text-align: center;
                            font-size: 14px;
                        }}
                        @media (max-width: 600px) {{
                            .container {{
                                margin: 10px;
                                border-radius: 8px;
                            }}
                            .content {{
                                padding: 20px 15px;
                            }}
                            .button {{
                                padding: 14px 28px;
                                font-size: 14px;
                            }}
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>🔐 Recuperación de Contraseña</h1>
                        </div>
                        
                        <div class='content'>
                            <div class='greeting'>
                                Hola <strong>{username}</strong>,
                            </div>
                            
                            <div class='message'>
                                Hemos recibido una solicitud para restablecer la contraseña de tu cuenta. 
                                Si no realizaste esta solicitud, por favor ignora este mensaje.
                            </div>
                
                            <div class='button-container'>
                                <a href='{recoveryLink}' class='button' target='_blank'>
                                    Restablecer Contraseña
                                </a>
                            </div>
                
                            <div class='expiration'>
                                ⏰ <strong>Este enlace expirará en {expirationHours} horas</strong>
                            </div>
                
                            <div class='warning'>
                                ⚠️ <strong>Importante:</strong> Por seguridad, nunca compartas este enlace con nadie. 
                                Nuestro equipo técnico nunca te pedirá tu contraseña por correo electrónico.
                            </div>
                
                            <div class='message'>
                                Si tienes problemas para hacer clic en el botón, copia y pega la siguiente URL en tu navegador:
                                <br>
                                <code style='word-break: break-all; color: #007bff;'>{recoveryLink}</code>
                            </div>
                        </div>
                        
                        <div class='footer'>
                            <p>© {DateTime.Now.Year} <strong>Tu Sistema</strong>. Todos los derechos reservados.</p>
                            <p>Este es un mensaje automático, por favor no respondas a este correo.</p>
                            <p>
                                <a href='#'>Política de Privacidad</a> | 
                                <a href='#'>Términos de Servicio</a> | 
                                <a href='#'>Soporte Técnico</a>
                            </p>
                        </div>
                    </div>
                </body>
                </html>
            ";
        }

        public static string GetWelcomeTemplate(string username, string loginLink)
        {
            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #28a745, #20c997); color: white; padding: 30px; text-align: center; }}
                        .content {{ background-color: #f8f9fa; padding: 30px; }}
                        .button {{ background-color: #007bff; color: white; padding: 15px 30px; text-decoration: none; border-radius: 5px; display: inline-block; }}
                        .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #6c757d; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>¡Bienvenido a nuestro sistema!</h1>
                        </div>
                        <div class='content'>
                            <p>Hola <strong>{username}</strong>,</p>
                            <p>¡Te damos la bienvenida a nuestra plataforma!</p>
                            <p>Tu cuenta ha sido creada exitosamente. Ahora puedes acceder al sistema:</p>
                            <p style='text-align: center;'>
                                <a href='{loginLink}' class='button'>Iniciar Sesión</a>
                            </p>
                        </div>
                        <div class='footer'>
                            <p>© {DateTime.Now.Year} Tu Sistema</p>
                        </div>
                    </div>
                </body>
                </html>
            ";
        }
    }
}