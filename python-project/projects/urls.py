#projects/urls.py
from django.conf.urls import patterns, url, include
from projects import views

urlpatterns = patterns('',
	url(r'^$', 'projects.views.customers', name='customers'),
	url(r'^new-customer/$', 'projects.views.new_customer', name='new_customer'),
	url(r'^customer-detail/(?P<slug>[\w-]+)/$', 'projects.views.customer_detail', name ='customer_detail'),
	url(r'^add-file/$', 'projects.views.add_file', name = 'add_file'),
	url(r'^create_post/$', 'projects.views.create_post', name = 'create_post'),
	)
