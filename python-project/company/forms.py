#company/forms.py
from __future__ import absolute_import
from django.forms import ModelForm
from .models import Company, Product, Contact


class CompanyForm(ModelForm):
	class Meta:
		model = Company
		fields = '__all__'





class ProductForm(ModelForm):
	class Meta:
		model = Product
		fields = '__all__'