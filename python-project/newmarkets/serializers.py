#newmarkets/serializers.py
from __future__ import unicode_literals
from rest_framework import serializers
from newmarkets.models import Country, HSProduct, IntracenImportData


class HSProductSerializer(serializers.ModelSerializer):

	class Meta:
		model = HSProduct
		fields = '__all__'


class CountrySerializer(serializers.ModelSerializer):

	class Meta:
		model = Country
		fields = '__all__'


class IntracenImportSerializer(serializers.ModelSerializer):

	class Meta:
		model = IntracenImportData
		fields = '__all__'