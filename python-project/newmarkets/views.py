#newmarkets/views.py

from django.contrib.sessions.models import Session
from django.shortcuts import render, redirect
from django.shortcuts import get_object_or_404
from django.http import *

from django.contrib.auth.decorators import login_required
from django.contrib.auth.models import User

from django.utils.decorators import method_decorator
from django.views.generic import TemplateView

from braces.views import LoginRequiredMixin

from company.models import Product
from newmarkets.models import *
from newmarkets.forms import *

from rest_framework import status
from rest_framework.decorators import api_view
from rest_framework.response import Response
from newmarkets.serializers import *

from easy_pdf.rendering import render_to_pdf_response
from easy_pdf.views import PDFTemplateView

#API endpoints
@api_view(['GET', 'POST'])
def api_country(request, country_id):
	country = Countries.objects.get(country_id=country_id)
	serializer = CountriesSerializer(country, many=False)
	return Response(serializer.data)


@api_view(['GET', 'POST'])
def api_gdpgrowth(request, country_id):
	gdp_growth = get_object_or_404(GDPGrowth, country_id=country_id)
	serializer = GDPGrowthSerializer(gdp_growth, many=False)
	return Response(serializer.data)


@api_view(['GET', 'POST'])
def api_results(request):
	if request.method == 'GET':
		resultsHS = request.session['name']#saving the user hs number in sessions
		#resultsHS = 2605
		#Getting the data from the database
		hsdescript = Products.objects.get(hs_number= resultsHS)
		top_five = Imports.objects.filter(hs_number= resultsHS)[0:6]
		serializer = ImportsSerializer(top_five, many=True)
		return Response(serializer.data)


class PDFReportView(LoginRequiredMixin, PDFTemplateView):

	template_name = "/newmarkets/pdfreport.html"

	def get_context_data(self, **kwargs):
	    return super(PDFReportView, self).get_context_data(
	        pagesize="Letter Landscape",
	        **kwargs
	    )


@login_required
def country(request, country_id):
	"Displays country specific data visualization"
	
	reportHS = request.session['name']

	markets = Imports.objects.filter(hs_number = reportHS)[1:6]
	country = Countries.objects.get(country_id=country_id)
	hsdescript = Products.objects.get(hs_number=reportHS)

	context_dict = {'country':country, 'markets': markets, 'hsdescript':hsdescript}
	return render(request, 'newmarkets/country.html', context_dict)


def hs6_to_hs4(hsnumber):
	if len(hsnumber) != 4:
		hsnumber=hsnumber[:4]
		return hsnumber
	return hsnumber


def results(request, resultsHS):
	"Display the overview of the recommended countries"
	resultsHS = hs6_to_hs4(resultsHS)
	print resultsHS
	request.session['name'] = resultsHS


	hsdescript = Products.objects.get(hs_number= resultsHS)
	top_five = Imports.objects.filter(hs_number= resultsHS)[1:6]
	context_dict = {'hsdescript':hsdescript, 'top_five':top_five}
	return render(request, 'newmarkets/results.html', context_dict)


@login_required
def pdf(request):
	"Generate Reports"
	resultsHS = request.session['name']
	#resultsHS = 2605
	#Getting the data from the database
	hsdescript = Products.objects.get(hs_number= resultsHS)
	top_five = Imports.objects.filter(hs_number= resultsHS)[1:6]
	context_dict = {'hsdescript':hsdescript, 'top_five':top_five}
	return render(request, 'newmarkets/pdf.html', context_dict)


@login_required
def feasability(request):
	"""
	We need to check first if the request.user is related to products

	"""
	try:
	    user_products = Product.objects.filter(user=request.user.id)
	    context_dict ={'user_products':user_products}
	except Product.DoesNotExist:
	    context_dict = {}
	return render(request, 'newmarkets/feasability.html', context_dict)


@login_required
def delete_product(request, resultsHS):
	"Delete view to delete product"
	pass

