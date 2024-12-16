import torch
import torch.nn as nn
import torchvision.models as models
import sys
import timm
import os
from PIL import Image
from pathlib import Path

class BryophytaClassifier(nn.Module):
  def __init__(self, classes_count):
    super(BryophytaClassifier, self).__init__()
    
    self.base_model = models.efficientnet_v2_m(weights='DEFAULT')
    if hasattr(self.base_model, 'fc'):
      in_features = self.base_model.fc.in_features
      self.base_model.fc = nn.Linear(in_features, classes_count)
    else:
      in_features = self.base_model.classifier[-1].in_features
      self.base_model.classifier[-1] = nn.Linear(in_features, classes_count)
  
  def forward(self, x):
    x = self.base_model(x)
    return x

model = BryophytaClassifier(classes_count=3)
current_path = Path(__file__).parent.absolute()
model.load_state_dict(torch.load(f'{current_path}/bryophyta_classifier_model.pt', weights_only=True, map_location=torch.device('cpu') ))

image_path = sys.argv[1]
result_file_name = sys.argv[2]

model_data_config = timm.data.resolve_model_data_config(model)
test_transform = timm.data.create_transform(**model_data_config, is_training=False)

img = Image.open(image_path)
transformed_img = test_transform(img).unsqueeze(0)

model.eval()
with torch.no_grad():
  output = model(transformed_img)
  prediction = torch.argmax(output, dim=1).cpu().numpy()[0] + 1 # +1 because the classes are 1, 2 and 3

os.rename(result_file_name, f'{result_file_name}{prediction}')