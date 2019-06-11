from django.conf.urls import patterns, url, include
from django.contrib import admin
from organizations.backends import invitation_backend, registration_backend

admin.autodiscover()

urlpatterns = patterns('',
    url(r'^admin/', include(admin.site.urls)),
    url(r'^organizations/', include('organizations.urls')),
    url(r'^invite/', include(invitation_backend().get_urls())),
    url(r'^register/', include(registration_backend().get_urls())),
)