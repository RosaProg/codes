#crm/forms.py
from __future__ import absolute_import
from django.forms import ModelForm
from .models import Lead, Interaction, People


class CustomerForm(ModelForm):
	class Meta:
		model = Lead
		exclude = ['user','time_stamp']


class PeopleForm(ModelForm):
	class Meta:
		model = People
		fields = ['contact_name', 'email', 'title', 'phone_number', 
					'skype', 'whatsapp']



class InteractionForm(ModelForm):
	class Meta:
		model = Interaction
		exclude = ['customer']