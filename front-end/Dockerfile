# Sử dụng hình ảnh Node.js làm cơ sở
FROM node:18

# Tạo thư mục làm việc trong container
WORKDIR /app

# Sao chép tất cả tệp và thư mục từ thư mục hiện tại của bạn vào /app trong container
COPY . .

# Cài đặt các phụ thuộc của ứng dụng
RUN npm install

# Xây dựng ứng dụng Vue.js
RUN npm run build

# Cổng mặc định mà ứng dụng Vue.js chạy là cổng 8080, bạn có thể điều chỉnh nó theo yêu cầu
EXPOSE 8080

# Lệnh để chạy ứng dụng khi container được khởi chạy
CMD ["npm", "run", "serve"]
