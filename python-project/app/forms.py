#exportabroad/forms.py
from __future__ import absolute_import
from django.forms import ModelForm
#from .models import ProfileModel
from company.models import Company
from myusers.models import ProfileModel

class ProfileForm(ModelForm):  # extending ModelForm, not Form as before
	class Meta:
		model = ProfileModel
		fields = '__all__'
		
class CompanyForm(ModelForm):
	class Meta:
		model = Company
		fields = '__all__'
			