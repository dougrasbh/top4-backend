import requests
import base64

with open("musgoExemplo.jpg", "rb") as img:
  f = img.read()
  img_base_64 = base64.b64encode(f)

requests.post('https://localhost:7091/api/Prediction/RequestPrediction', {'imageBytesInBase64': img_base_64, 'userId': 1})