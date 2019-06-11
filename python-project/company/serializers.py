#company/serializers.py
from __future__ import unicode_literals
from rest_framework import serializers
from . models import Company, Product, Contact


class CompanySerializer(serializers.ModelSerializer):

	class Meta:
		model = Company
		fields = '__all__'


class ProductSerializer(serializers.ModelSerializer):

	class Meta:
		model = Product
		fields = '__all__'


class ContactSerializer(serializers.ModelSerializer):

	class Meta:
		model = Contact
		fields = '__all__'



