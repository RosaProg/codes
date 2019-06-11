angular.module('ie7-support', []).config(function($sceProvider) {
  // Completely disable SCE to support IE7.
  $sceProvider.enabled(false);
});