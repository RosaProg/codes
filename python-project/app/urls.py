#urls.py
from __future__ import unicode_literals

from django.conf import settings
from django.conf.urls import include, url, patterns
from django.conf.urls.static import static
from django.contrib import admin
from django.views.generic import TemplateView
from organizations.backends import invitation_backend
import allauth
from . import views
from sitemaps import ExportAbroadSitemap
from rest_framework.routers import DefaultRouter
from company.views import CustomerViewSet
from organizations.backends import invitation_backend, registration_backend

admin.autodiscover()

sitemaps = {
    'export': ExportAbroadSitemap(['index', ])
}

#Create a router and register our viewsets with it
router = DefaultRouter()
router.register(r'company/customers', CustomerViewSet, 'customers')

urlpatterns = [
    #Static Website
    url(r'^$', TemplateView.as_view(template_name="home.html"), name="home"),
    url(r'^product/$', TemplateView.as_view(template_name="product.html"), name='product'),
    url(r'^about/$', TemplateView.as_view(template_name="team.html"), name='about'),
    url(r'^pricing/$', TemplateView.as_view(template_name="pricing.html"), name='pricing'),
    url(r'^contact/$', TemplateView.as_view(template_name="contact.html"), name='contact'),

    #Dashboard
    url(r'^dashboard/$', views.dashboard, name='dashboard'), 
    #Promotion, landing pages start here 
    url(r'^promo/$', views.promo, name = 'promo'),
    url(r'^top-countries/$', views.topcountries, name = 'topcountries'),
    url(r'^top-countries-2/$', views.topcountries2, name = 'topcountries2'),

    url(r'^sitemap\.xml$', 'django.contrib.sitemaps.views.sitemap', {'sitemaps': sitemaps}),

    #Api DRF includes
    url(r'^api/v1/', include(router.urls, namespace='api/v1')),
    url(r'^api-auth/', include('rest_framework.urls', namespace='rest_framework')),
    url(r'^docs/', include('rest_framework_docs.urls')),

    #Including urls from all other packages
    url(r'^newmarkets/', include('newmarkets.urls')),
    url(r'^admin/', include(admin.site.urls)),
    url(r'^accounts/', include('allauth.urls')),
	url(r'^accounts/settings/$', views.settings, name="settings"),
	url(r'^accounts/settings/company/$', views.settings_company, name="settings_company"),
	url(r'^accounts/settings/profile/$', views.settings_profile, name="settings_profile"),
	url(r'^accounts/settings/users_permission/$', views.users_permission, name="users_permission"),
	url(r'^accounts/settings/organization_admin/$', views.organization_admin, name="organization_admin"),

    #URL config for django-organizations
    url(r'^organizations/', include('organizations.urls')),
    url(r'^invitations/', include(invitation_backend().get_urls())),
	url(r'^register/', include(registration_backend().get_urls())),
    url(r'^customers-organization/', include('customers-organization.urls')),
    url(r'^organizations-advisor/', include('organizations-advisor.urls')),
    url(r'^customers-employees/', include('customers-employees.urls')),
    url(r'^organizations-employees/', include('organizations-employees.urls')),

    url(r'^media/(?P<path>.*)$', 'django.views.static.serve',
        {'document_root': settings.MEDIA_ROOT}),
]

if settings.DEBUG:
	urlpatterns += patterns('django.contrib.staticfiles.views',
		(r'^static/(?P<path>.*)$',
		'serve',),
        )
