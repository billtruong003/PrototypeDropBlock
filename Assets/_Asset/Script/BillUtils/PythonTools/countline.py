import os

def count_lines_of_code(directory, file_extension, excluded_dirs):
    folder_line_counts = {}
    
    for root, dirs, files in os.walk(directory):
        # Bỏ qua các thư mục trong danh sách loại trừ
        dirs[:] = [d for d in dirs if not any(excluded_dir in os.path.join(root, d) for excluded_dir in excluded_dirs)]
        
        folder_line_count = 0
        for file in files:
            if file.endswith(file_extension):
                try:
                    with open(os.path.join(root, file), 'r', encoding='utf-8') as f:
                        for line in f:
                            if line.strip():  # Chỉ đếm các dòng không trống
                                folder_line_count += 1
                except Exception as e:
                    print(f"Could not read file {file}: {e}")
        
        if folder_line_count > 0:
            folder_line_counts[root] = folder_line_count
    
    return folder_line_counts

# Set the directory to the current directory
directory = '.'  # Thư mục hiện tại
file_extension = '.cs'  # Định dạng file muốn đếm
excluded_dirs = ['NaughtyAttributes', 'PackageAsset', 'TextMesh Pro', 'DOTween']  # Các thư mục cần loại trừ

folder_line_counts = count_lines_of_code(directory, file_extension, excluded_dirs)

# In ra số dòng code trong từng thư mục
total_lines = 0
for folder, line_count in folder_line_counts.items():
    print(f'{folder}: {line_count} lines of code')
    total_lines += line_count

print(f'Total lines of code: {total_lines}')
