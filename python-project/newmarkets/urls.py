#newmarkets/urls.py

from django.conf.urls import patterns, url
from wkhtmltopdf.views import PDFTemplateView

from newmarkets import views
from newmarkets.views import PDFReportView


urlpatterns = patterns('',

	url(r'^results/(?P<resultsHS>[0-9]{4,8})/$', views.results, name='results'),
	url(r'^country/(?P<country_id>[A-Z]{3})/$', views.country, name='country'),
	url(r'^feasability/$', views.feasability, name='feasability'),

	#api call
	url(r'^api/results/$', views.api_results),
	url(r'^api/results/(?P<country_id>[A-Z]{3})/$', views.api_country),
	url(r'^api/gdpgrowth/(?P<country_id>[A-Z]{3})/$', views.api_gdpgrowth),
	
	#PDF reports generation
	url(r'^pdf/$', 'newmarkets.views.pdf', name='pdf'),
	
	)
