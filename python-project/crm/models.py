#crm/models.py
from autoslug import AutoSlugField
from decimal import Decimal
from model_utils import Choices
#Hoping to introduce twilio with this
from phonenumber_field.modelfields import PhoneNumberField

from django.contrib.auth.models import User
from django.db import models
from django_countries.fields import CountryField


class Lead(models.Model):
    """
    A customer is really a lead that can be categorized as follow:
    prospect, contacted, qualified, customer.
    holy cow.
    """
    user = models.ForeignKey(User)
    #company crucial info
    company_name =  models.CharField(max_length=300)
    slug = AutoSlugField(populate_from='company_name', blank=True, null=True)
    website = models.URLField(max_length=200, null=True, blank=True)
    address1 = models.CharField(max_length=300, null=True, blank=True)
    address2 = models.CharField(max_length=300, null=True, blank=True)
    city = models.CharField(max_length=100)
    state_province = models.CharField(max_length=300, null=True, blank=True)
    postal_code = models.CharField(max_length=10, null=True, blank=True)
    country = CountryField()
    STATUS = Choices('prospect', 'contacted', 'qualified', 'Customer')
    status = models.CharField(choices=STATUS, default=STATUS.prospect, 
            max_length=20)
    value = models.DecimalField(max_digits=10, decimal_places=2, 
            default=Decimal('0.00'))
    time_stamp = models.DateTimeField(auto_now_add=True)

    def __unicode__(self):
        return self.company_name



class People(models.Model):
    """
    This class represent people who are associated with the company. 
    Contact names
    """
    company = models.ForeignKey(Lead)
    contact_name = models.CharField(max_length=300)
    email = models.EmailField(null=True, blank=True)
    title = models.CharField(max_length=300, null=True, blank=True)
    phone_number = PhoneNumberField(blank=True)
    skype = models.CharField(max_length=50, null=True, blank=True)
    whatsapp = models.CharField(max_length=50, null=True, blank=True)
    time_stamp = models.DateTimeField(auto_now_add=True)
    
INTERACTION_TYPE = (
    (1, u'Phone Call'),
    (2, u'Email' ),
    )


class Interaction(models.Model):
    """what happened between you and the customer?"""

    when = models.DateTimeField(auto_now_add=True)
    kind = models.IntegerField(choices=INTERACTION_TYPE)
    lead = models.ForeignKey(Lead)
    note = models.TextField(blank=True, null=True)