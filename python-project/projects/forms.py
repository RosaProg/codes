#projects/forms.py
from __future__ import absolute_import
from django import forms
from .models import Customer, Contact, Attachment, Post

class CustomerForm(forms.ModelForm):
	class Meta:
		model = Customer
		exclude = ['user', 'slug', 'created_at', 'modified_at']

class ContactForm(forms.ModelForm):
    class Meta:
        model = Contact
        fields = ['contact_name', 
                    'email', 
                    'title', 
                    'phone_number', 
                    'skype', 
                    'whatsapp']

class AttachmentForm(forms.ModelForm):
	class Meta:
		model = Attachment
		fields = ['file_name', 'description', 'attached_file']


class PostForm(forms.ModelForm):
    class Meta:
        model = Post
        # exclude = ['author', 'updated', 'created', ]
        fields = ['text']
        widgets = {
            'text':forms.TextInput(
                attrs = {'id': 'post-text', 'required':True, 'placeholder': 'say something...'}
            ),
        }
