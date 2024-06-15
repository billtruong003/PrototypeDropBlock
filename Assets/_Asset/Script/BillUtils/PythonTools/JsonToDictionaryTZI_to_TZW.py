import os
import json

# Đường dẫn đến tệp JSON chứa dữ liệu timezone
json_file_path = 'Assets/Resources/timezone_mapping.json'

# Hàm kiểm tra sự tồn tại của tệp và đọc dữ liệu từ tệp JSON
def load_timezone_data(file_path):
    if not os.path.exists(file_path):
        print(f"File {file_path} does not exist.")
        return None
    
    with open(file_path, 'r') as file:
        data = json.load(file)
    return data

# Hàm tạo mã C# từ dữ liệu timezone
def generate_csharp_code(timezone_data):
    if timezone_data is None:
        print("No data to generate code.")
        return

    print("#region CONFIG TIME ZONE")
    print("private static readonly Dictionary<string, string> IanaToWindowsTimeZoneMap = new Dictionary<string, string>")
    print("{")
    
    for entry in timezone_data:
        iana = entry["Iana"]
        windows = entry["Windows"]
        print(f'    {{ "{iana}", "{windows}" }},')
    
    print("};")
    print("#endregion")

# Đọc dữ liệu từ tệp JSON
timezone_data = load_timezone_data(json_file_path)

# Tạo mã C#
generate_csharp_code(timezone_data)
