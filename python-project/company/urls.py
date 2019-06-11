#company/urls
from __future__ import unicode_literals
from django.conf.urls import url
from company import views
from company.views import CustomersList

urlpatterns = [
	url(r'^$', CustomersList.as_view()),
	url(r'^add_company/$', views.general_info, name='general_info'), 
	url(r'^economic/$', views.economic_info, name='economic_info'),
	url(r'^add_product/$', views.product_info, name='product_info'),
	]
