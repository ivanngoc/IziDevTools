sudo systemctl daemon-reload
sudo systemctl stop easy-switcher
sudo systemctl disable easy-switcher
sudo systemctl daemon-reload
sudo easy-switcher -i
sudo easy-switcher -c
sudo systemctl enable easy-switcher
sudo systemctl start easy-switcher