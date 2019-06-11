#company/views.py
from __future__ import absolute_import

from django.contrib.auth.decorators import login_required
from django.contrib.auth.models import User
from django.contrib.sessions.models import Session
from django.views.generic import ListView 
from django.shortcuts import render, redirect, get_object_or_404

from braces.views import LoginRequiredMixin
from rest_framework import viewsets
from rest_framework.decorators import detail_route

from .models import Company, Product, Contact
from .serializers import CompanySerializer, ProductSerializer, ContactSerializer
from .forms import CompanyForm, ProductForm
from newmarkets.views import feasability
	

class CustomersList(LoginRequiredMixin, ListView):
	"""
	Renders the list of companies(customers)associated with an 
	Export Advisor (user)
	
	"""
	template_name = "company/customers.html"
	context_object_name = "customers"

	def get_queryset(self):
		return Company.objects.filter(user=self.request.user)


class CustomerViewSet(viewsets.ModelViewSet):
	"""
	This Viewset automatically provides 'list', 'create', 'retrieve',
	'update', and 'destroy' actions

	Additionally, we also provide an extra 'highlight' action.

	"""
	serializer_class = CompanySerializer
	def get_queryset(self):
		return Company.objects.filter(user=self.request.user)

@login_required
def general_info(request):
	"Company onBoroading:Gathering more info about the company"
	if request.method == "POST":
		form = CompanyForm(request.POST)
		if form.is_valid():
			new_company = form.save(commit=False)
			new_company.user = request.user
			request.session['company_id'] =new_company.company_name
			new_company.save()
			return redirect(economic_info)
		else:
			print form.errors
	#If request is a 'GET'		
	else:
		form = CompanyForm()

	context_dict = {'form':form}		
	return render (request, 'company/general_info.html', context_dict)


@login_required
def economic_info(request):
	"Company onBoarding: Gathering economic info"
	if request.method == "POST":
		form = CompanyEconInfoForm(request.POST)
		if form.is_valid():
			form.save(commit=True)
			return redirect(product_info)
		else:
			print form.errors
	#If request is a 'GET'		
	else:
		form =CompanyEconInfoForm()

	context_dict = {'form':form}
	return render(request, 'company/economic_info.html', context_dict)


@login_required
def product_info(request):
	"Product onBoarding: Gathering information on the product"
	if request.method == "POST":
		form = ProductForm(request.POST)
		if form.is_valid():
			new_product = form.save(commit=False)
			new_product.user =request.user

			form.save()
			return redirect(feasability)
		else:
			print form.errors
	#If request is a 'GET'		
	else:
		form =ProductForm()
	context_dict = {'form':form}
	return render(request, 'company/product_info.html', context_dict)