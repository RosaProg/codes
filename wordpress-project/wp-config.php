<?php
/** 
 * Configuración básica de WordPress.
 *
 * Este archivo contiene las siguientes configuraciones: ajustes de MySQL, prefijo de tablas,
 * claves secretas, idioma de WordPress y ABSPATH. Para obtener más información,
 * visita la página del Codex{@link http://codex.wordpress.org/Editing_wp-config.php Editing
 * wp-config.php} . Los ajustes de MySQL te los proporcionará tu proveedor de alojamiento web.
 *
 * This file is used by the wp-config.php creation script during the
 * installation. You don't have to use the web site, you can just copy this file
 * to "wp-config.php" and fill in the values.
 *
 * @package WordPress
 */

// ** Ajustes de MySQL. Solicita estos datos a tu proveedor de alojamiento web. ** //
/** El nombre de tu base de datos de WordPress */
define('WP_CACHE', true); //Added by WP-Cache Manager
define( 'WPCACHEHOME', 'C:\xampp\htdocs\trademdesign\wp-content\plugins\wp-super-cache/' ); //Added by WP-Cache Manager
define('DB_NAME', 'tradem_design');

/** Tu nombre de usuario de MySQL */
define('DB_USER', 'root');

/** Tu contraseña de MySQL */
define('DB_PASSWORD', '');

/** Host de MySQL (es muy probable que no necesites cambiarlo) */
define('DB_HOST', 'localhost');

/** Codificación de caracteres para la base de datos. */
define('DB_CHARSET', 'utf8');

/** Cotejamiento de la base de datos. No lo modifiques si tienes dudas. */
define('DB_COLLATE', '');

/**#@+
 * Claves únicas de autentificación.
 *
 * Define cada clave secreta con una frase aleatoria distinta.
 * Puedes generarlas usando el {@link https://api.wordpress.org/secret-key/1.1/salt/ servicio de claves secretas de WordPress}
 * Puedes cambiar las claves en cualquier momento para invalidar todas las cookies existentes. Esto forzará a todos los usuarios a volver a hacer login.
 *
 * @since 2.6.0
 */
define('AUTH_KEY',         'EU,6Z5LtfBG=%^# &%+LC~csYxU_2FW_D^!mC?|_#9`=d@xLoQCBt4%,E>9^#w?A');
define('SECURE_AUTH_KEY',  'td`(-YFJD^7}-1]-PbxWBEj*Y/X)-pO$lsK|WpI-{cs~mR[ HMmOH:fOZVO+tVuW');
define('LOGGED_IN_KEY',    '@`x-3e}+$@um||hJ-.#q{bm{)u-b&NY:8c30OK:S6f-_2P_-?kX~;<4+:fIfKEaI');
define('NONCE_KEY',        ':XUe!;~Yl:$]qNM>r`S:=ZLZU/6-v^l=+n2nHju|lYqOBG$kqg[)te*S1f[+2gPO');
define('AUTH_SALT',        '&+Zqyu$<&Tp-$]3pp#AW7#fjkFWe$Wsa+BP;-Nbs1w>&9|e#-Lr;(&^S,zQ1gkU,');
define('SECURE_AUTH_SALT', 'O630LEWNCTxQ5YTmq4Rbx3d,Z}]p%J+2Ee;rjvuulI+QQu p!Vp/l<~)&4sq9L*l');
define('LOGGED_IN_SALT',   'Gn+_gcfa5Icf2;3.Lawi~a~M`Fz+&jpZ(~TIoQ*;v3rQj<mHQ79^X(c#C9._GE~U');
define('NONCE_SALT',       ']0UescJv`BA_Ou./ 4qR%|@tpHUIRi{GrwY_hh?IRVWFXJlhEhAMi(Q[~*yZW3}w');

/**#@-*/

/**
 * Prefijo de la base de datos de WordPress.
 *
 * Cambia el prefijo si deseas instalar multiples blogs en una sola base de datos.
 * Emplea solo números, letras y guión bajo.
 */
$table_prefix  = 'wp_';


/**
 * Para desarrolladores: modo debug de WordPress.
 *
 * Cambia esto a true para activar la muestra de avisos durante el desarrollo.
 * Se recomienda encarecidamente a los desarrolladores de temas y plugins que usen WP_DEBUG
 * en sus entornos de desarrollo.
 */
define('WP_DEBUG', false);

/* ¡Eso es todo, deja de editar! Feliz blogging */

/** WordPress absolute path to the Wordpress directory. */
if ( !defined('ABSPATH') )
	define('ABSPATH', dirname(__FILE__) . '/');

/** Sets up WordPress vars and included files. */
require_once(ABSPATH . 'wp-settings.php');

