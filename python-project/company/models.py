#company/models.py
"""
This app stores all our customers company information.
General information and economic information.
"""
from __future__ import unicode_literals

from django.db import models
from django.contrib.auth.models import User
from django_countries.fields import CountryField

from phonenumber_field.modelfields import PhoneNumberField
from autoslug import AutoSlugField
from decimal import Decimal
from model_utils import Choices
from model_utils.fields import StatusField
from model_utils.models import TimeStampedModel
from organizations.models import (Organization, OrganizationUser,
    OrganizationOwner)

class Company(Organization):
	"Company's general information"
	user = models.ForeignKey(User)
	#name = models.CharField(max_length=128)
	description = models.TextField(blank=True, max_length=250)
	#slug = AutoSlugField(populate_from='name', unique_with='id', null=True)

	website = models.URLField(max_length=200, blank=True)
	address_line1 = models.CharField(max_length=100)
	address_line2 = models.CharField(blank=True, max_length=100)
	city = models.CharField(max_length=50)
	state_province = models.CharField(max_length=50, blank=True)
	postal_code = models.CharField(max_length=50)
	country = CountryField()
	STATUS = Choices (
		'Chemical', 
		'Mining',
		'Food',
		'Basic Materials',
		'Services',
		'Transportation',
		'Healthcare',
		'Technology',
		'Communication',
		'manufacturing'
		)
	industry = StatusField(default=STATUS.manufacturing)

	created_at = models.DateTimeField(auto_now_add=True)
	updated_at = models.DateTimeField(auto_now=True, null=True)

	def __unicode__(self):
		return self.name


class Product(models.Model):
	"""
	Product line for the company
	"""

	company = models.ForeignKey(Company)
	user = models.ForeignKey(User)

	product_name = models.CharField(max_length=128)
	product_description = models.TextField(blank=True, max_length=500)
	product_hs_code = models.CharField(max_length=15, null=True)


	cost_goods_sold = models.DecimalField(max_digits=10, decimal_places=2, default=0.00)
	wholesale_price = models.DecimalField(max_digits=10, decimal_places=2, default=0.00)
	retail_price = models.DecimalField(max_digits=9, decimal_places=2, default=0.00)
	daily_production = models.DecimalField(max_digits=9, decimal_places=2, default=0.00)

	created_at = models.DateTimeField(auto_now_add=True)
	updated_at = models.DateTimeField(auto_now=True, null=True)

	def __unicode__(self):
		return self.product_name


class Contact(models.Model):
	"""
	This class represent people who are associated with the company. 
	Contact names
	"""
	company = models.ForeignKey(Company)
	user = models.ForeignKey(User)
	first_name = models.CharField(max_length=300)
	last_name = models.CharField(max_length=300)
	email = models.EmailField(blank=True)
	title = models.CharField(max_length=300, blank=True)
	phone_number = PhoneNumberField(blank=True)
	skype = models.CharField(max_length=50, blank=True)
	whatsapp = models.CharField(max_length=50, blank=True)

	created_at = models.DateTimeField(auto_now_add=True)
	updated_at = models.DateTimeField(auto_now=True, null=True)

	def __unicode__(self):
		return self.last_name
