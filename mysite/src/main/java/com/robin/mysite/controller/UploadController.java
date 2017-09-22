package com.robin.mysite.controller;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.multipart.MultipartFile;
import org.springframework.web.servlet.mvc.support.RedirectAttributes;

import javax.servlet.http.HttpServletResponse;
import java.io.*;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

@Controller
public class UploadController {
    private static final Logger LOG = LoggerFactory.getLogger(UploadController.class);

    private static String TMP_FOLDER = "/tmp/";

    private String filePath = "";

    @GetMapping("/month")
    public String index() {
        return "upload";
    }

    @PostMapping("/upload")
    public String singleFileUpload(@RequestParam("file") MultipartFile file, RedirectAttributes redirectAttributes) {
        if (file.isEmpty()) {
            redirectAttributes.addFlashAttribute("message", "Please select a file to upload");
            return "redirect:/uploadStatus";
        }

        String upload_folder = TMP_FOLDER + "month";
        try {
            File dirUpload = new File(upload_folder);
            if (dirUpload.exists() && dirUpload.isDirectory()) {
                Files.delete(dirUpload.toPath());
            }

            Files.createDirectory(dirUpload.toPath());
        } catch (IOException e) {
            e.printStackTrace();
        }
        try {
            byte[] bytes = file.getBytes();
            String fullPath = upload_folder + file.getOriginalFilename();

            this.filePath = fullPath;

            Path path = Paths.get(fullPath);
            Files.write(path, bytes);

            redirectAttributes.addFlashAttribute("message", "You successfully uploaded " + fullPath);
        } catch (IOException e) {
            e.printStackTrace();
        }

        return "redirect:/uploadStatus";
    }

    @GetMapping("/uploadStatus")
    public String uploadStatus() {
        return "uploadStatus";
    }

    @GetMapping("/download")
    public void download(HttpServletResponse resp) {
        LOG.info("to download {}", filePath);
        File file = new File(filePath);
        resp.setHeader("content-type", "application/octet-stream");
        resp.setContentType("application/octet-stream");
        resp.setHeader("Content-Disposition", "attachment;filename=" + filePath);

        byte[] buff = new byte[1024];
        BufferedInputStream bis = null;
        OutputStream os = null;
        try {
            os = resp.getOutputStream();
            bis = new BufferedInputStream(new FileInputStream(file));
            int i = bis.read(buff);
            while (i != -1) {
                os.write(buff, 0, buff.length);
                os.flush();
                i = bis.read(buff);
            }
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            if (bis != null) {
                try {
                    bis.close();
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }

        }
    }
}
