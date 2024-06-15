import requests
import json
import os
import time

# Hàm lấy danh sách IANA Time Zones từ TimeZoneDB
def fetch_iana_timezones(api_key):
    url = f"http://api.timezonedb.com/v2.1/list-time-zone?key={api_key}&format=json"
    response = requests.get(url)
    if response.status_code == 200:
        data = response.json()
        return [zone['zoneName'] for zone in data['zones']]
    else:
        print(f"Error fetching IANA Time Zones: {response.status_code}")
        return []

# Hàm chuyển đổi IANA Time Zones sang Windows Time Zones
def get_windows_timezone(iana_timezone, api_key):
    try:
        # Chúng ta sử dụng giả định rằng API TimeZoneDB trả về Windows Time Zone trong response
        url = f"http://api.timezonedb.com/v2.1/get-time-zone?key={api_key}&format=json&by=zone&zone={iana_timezone}"
        response = requests.get(url)
        if response.status_code == 200:
            data = response.json()
            print(f"success {response.json()}")
            return data.get('abbreviation')  # Giả sử abbreviation là Windows Time Zone
        elif response.status_code == 429:
            print(f"Rate limit exceeded for {iana_timezone}. Waiting before retrying...")
            time.sleep(5)  # Đợi 5 giây trước khi thử lại
            return get_windows_timezone(iana_timezone, api_key)
        else:
            print(f"Error converting IANA to Windows Time Zone for {iana_timezone}: {response.status_code}")
            return None
    except Exception as e:
        print(f"Exception occurred: {str(e)}")
        return None

# Hàm tạo danh sách ánh xạ từ IANA sang Windows Time Zones
def create_timezone_mappings(api_key):
    iana_timezones = fetch_iana_timezones(api_key)
    timezone_mappings = []

    for iana in iana_timezones:
        windows = get_windows_timezone(iana, api_key)
        if windows:
            timezone_mappings.append({"Iana": iana, "Windows": windows})
        else:
            print(f"Mapping not found for IANA Time Zone: {iana}")

    return timezone_mappings

# Hàm ghi dữ liệu vào tệp JSON
def write_to_json(data, filepath):
    directory = os.path.dirname(filepath)
    if not os.path.exists(directory):
        os.makedirs(directory)

    with open(filepath, 'w') as json_file:
        json.dump(data, json_file, indent=4)
    print(f"Data written to {filepath}")

# Đường dẫn tới tệp JSON
json_file_path = "Assets/Resources/timezone_mapping.json"

# Chạy các hàm để tạo danh sách và lưu vào tệp JSON
api_key = "PI5M6ZSB0FR3"
timezone_mappings = create_timezone_mappings(api_key)
write_to_json(timezone_mappings, json_file_path)
